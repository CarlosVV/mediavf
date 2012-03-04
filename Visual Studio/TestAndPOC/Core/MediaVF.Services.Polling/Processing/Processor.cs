using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using MediaVF.Services.Polling.Content;

namespace MediaVF.Services.Polling.Processing
{
    public abstract class ContentProcessor : IContentProcessor
    {
        #region Properties

        #region Background Thread

        /// <summary>
        /// Indicates if content processing can be cancelled
        /// </summary>
        protected virtual bool AllowsCancellation { get; set; }

        /// <summary>
        /// Indicates if content processing reports progress
        /// </summary>
        protected virtual bool ReportsProgress { get; set; }

        /// <summary>
        /// Runs processing logic on a background thread
        /// </summary>
        BackgroundWorker _processWorker;
        protected BackgroundWorker ProcessWorker
        {
            get
            {
                if (_processWorker == null)
                {
                    // create background thread and attach handlers
                    _processWorker = new BackgroundWorker();
                    _processWorker.DoWork += new DoWorkEventHandler(ProcessWorker_DoWork);
                    _processWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProcessWorker_RunWorkerCompleted);

                    // set whether or not the worker supports cancellation
                    _processWorker.WorkerSupportsCancellation = AllowsCancellation;

                    // set whether or not the worker reports progress
                    _processWorker.WorkerReportsProgress = ReportsProgress;
                    if (ReportsProgress)
                        _processWorker.ProgressChanged += new ProgressChangedEventHandler(ProcessWorker_ProgressChanged);
                }

                return _processWorker;
            }
        }

        #endregion

        #region Content Queue

        /// <summary>
        /// Synchronizes access to the queue
        /// </summary>
        object _queueLock;
        object QueueLock
        {
            get
            {
                if (_queueLock == null)
                    _queueLock = new object();
                return _queueLock;
            }
        }

        /// <summary>
        /// Queues up content to be processed
        /// </summary>
        Queue<IContent> _contentQueue;
        Queue<IContent> ContentQueue
        {
            get
            {
                if (_contentQueue == null)
                    _contentQueue = new Queue<IContent>();
                return _contentQueue;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Processing

        /// <summary>
        /// Processes content, first checking if content is valid, then running processing on background thread
        /// </summary>
        /// <param name="content"></param>
        public void Process(List<IContent> content)
        {
            // process each content item
            content.ForEach(singleContentItem =>
            {
                // check that the item can be processed by this processor
                if (CanProcess(singleContentItem.GetType()))
                {
                    // check if processor is already running a diff item
                    if (ProcessWorker.IsBusy)
                    {
                        // lock queue and add item
                        lock (QueueLock)
                        {
                            ContentQueue.Enqueue(singleContentItem);
                        }
                    }
                    else
                        // process item on background thread
                        ProcessWorker.RunWorkerAsync(singleContentItem);
                }
            });
        }

        /// <summary>
        /// Indicates whether a type can be processed by this processor
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool CanProcess(Type type)
        {
            // get the content processing attributes on this type
            List<ContentProcessorAttribute> contentProcessorAttributes =
                GetType().GetCustomAttributes(typeof(ContentProcessorAttribute), true).Cast<ContentProcessorAttribute>().ToList();
            
            // check that there is an attribute for this content type
            return contentProcessorAttributes != null && contentProcessorAttributes.Count > 0 && contentProcessorAttributes.Any(attrib => attrib.ContentType == type);
        }

        /// <summary>
        /// Runs content item processing on background thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProcessWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // get content
            IContent content = e.Argument as IContent;

            // if content is not null, process it
            if (content != null)
                ProcessItem(content);
        }

        /// <summary>
        /// Allow defined classes to initialize themselves
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Allow derived classes to define what processing is done on the content
        /// </summary>
        /// <param name="content"></param>
        protected abstract void ProcessItem(IContent content);

        #endregion

        #region Progress Tracking

        /// <summary>
        /// Handles progress reporting from background processing of an item
        /// </summary>
        /// <param name="sender">The worker reporting progress</param>
        /// <param name="e">The progress event args</param>
        void ProcessWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnProgressChanged(e);
        }

        /// <summary>
        /// Allow derived classes to provide progress handling
        /// </summary>
        /// <param name="e">The progress event args</param>
        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
        }

        #endregion

        #region Completion

        /// <summary>
        /// Handles completion of background processing of an item
        /// </summary>
        /// <param name="sender">The worker that has completed</param>
        /// <param name="e">The completion event args</param>
        void ProcessWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // allow derived processing of completion of an item
            OnCompleted(e);

            // get th next item on the queue, if any
            lock (QueueLock)
            {
                if (ContentQueue.Count > 0)
                    ProcessWorker.RunWorkerAsync(ContentQueue.Dequeue());
            }
        }

        /// <summary>
        /// Allow derived classes to provide additional completion handling
        /// </summary>
        /// <param name="e">The completion event args</param>
        protected virtual void OnCompleted(RunWorkerCompletedEventArgs e)
        {
        }

        #endregion

        #endregion
    }
}
