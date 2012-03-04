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
                    _processWorker = new BackgroundWorker();
                    _processWorker.DoWork += new DoWorkEventHandler(ProcessWorker_DoWork);
                    _processWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProcessWorker_RunWorkerCompleted);

                    _processWorker.WorkerSupportsCancellation = AllowsCancellation;

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
            IContent content = e.Argument as IContent;

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

        void ProcessWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnProgressChanged(e);
        }

        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
        }

        #endregion

        #region Completion

        void ProcessWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnCompleted(e);

            lock (QueueLock)
            {
                if (ContentQueue.Count > 0)
                    ProcessWorker.RunWorkerAsync(ContentQueue.Dequeue());
            }
        }

        protected virtual void OnCompleted(RunWorkerCompletedEventArgs e)
        {
        }

        #endregion

        #endregion
    }
}
