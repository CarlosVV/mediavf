using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Unity;

using MediaVF.Services.Data;
using MediaVF.Services.Components;
using MediaVF.Services.ArtistTrack.Polling.Content;
using MediaVF.Services.ArtistTrack.Polling.Processing;

namespace MediaVF.Services.ArtistTrack.Polling.Matching
{
    [ContentProcessor(typeof(TextContent))]
    [ContentProcessor(typeof(HtmlContent))]
    [ContentProcessor(typeof(XmlContent))]
    public class ParentChildMatchingProcessor<TParent, TChild> : ContentProcessor
    {
        #region Properties

        /// <summary>
        /// Gets the name of the matching processor
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Gets or sets the service component to which this processor belongs
        /// </summary>
        IServiceComponent ServiceComponent { get; set; }

        /// <summary>
        /// Gets or sets the parent object matcher
        /// </summary>
        ITextMatcher<TParent> ParentObjectMatcher { get; set; }

        /// <summary>
        /// Gets or sets the child object matcher
        /// </summary>
        ITextMatcher<TChild> ChildObjectMatcher { get; set; }

        /// <summary>
        /// Gets or sets the parent property
        /// </summary>
        PropertyInfo ParentProperty { get; set; }

        /// <summary>
        /// Gets or sets the child property
        /// </summary>
        PropertyInfo ChildProperty { get; set; }

        /// <summary>
        /// Gets or sets a delegate for handling child objects that are found
        /// </summary>
        public Action<IEnumerable<TChild>> ChildObjectHandler { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the parent-child matching processor for the given service component, data manager, and processor name
        /// </summary>
        /// <param name="serviceComponent">The service component to which this processor belongs</param>
        /// <param name="dataManager">The data manager for this processor</param>
        /// <param name="name">The name of the processor</param>
        public ParentChildMatchingProcessor(IServiceComponent serviceComponent, string name)
        {
            ServiceComponent = serviceComponent;
            Name = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the processor
        /// </summary>
        public override void Initialize()
        {
            // resolve the parent type using the component container and initialize it
            ParentObjectMatcher = ServiceComponent.ComponentContainer.Resolve<ITextMatcher<TParent>>(Name);
            if (ParentObjectMatcher != null)
                ParentObjectMatcher.Initialize();

            // resolve the child type using the component container and initialize it
            ChildObjectMatcher = ServiceComponent.ComponentContainer.Resolve<ITextMatcher<TChild>>(Name);
            if (ChildObjectMatcher != null)
                ChildObjectMatcher.Initialize();
        }

        /// <summary>
        /// Processes a content item
        /// </summary>
        /// <param name="content">The content to process</param>
        protected override void ProcessItem(IContent content)
        {
            // get the type of the content
            Type typeOfContent = content.GetType();

            // get the item as text
            string regexMatchText = string.Empty;
            if (typeOfContent == typeof(TextContent))
                regexMatchText = ((TextContent)content).Content;
            else if (typeOfContent == typeof(HtmlContent))
                regexMatchText = ((HtmlContent)content).Content.ToString();
            else if (typeOfContent == typeof(XmlContent))
                regexMatchText = ((XmlContent)content).Content.ToString();

            if (!string.IsNullOrEmpty(regexMatchText))
            {
                List<TChild> childObjs = CheckForMatches(regexMatchText);

                if (ChildObjectHandler != null)
                    ChildObjectHandler(childObjs);
            }
        }

        /// <summary>
        /// Sets the link between the parent and the child, given the names of the properties on the parent and child that link them
        /// </summary>
        /// <param name="parentPropertyName">The name of the linking property on the parent</param>
        /// <param name="childPropertyName">The name of the linking property on the child</param>
        public void SetParentChildLink(string parentPropertyName, string childPropertyName)
        {
            // get the property on the parent type
            if (typeof(TParent).GetProperty(parentPropertyName) != null)
                ParentProperty = typeof(TParent).GetProperty(parentPropertyName);

            // get the property on the child type
            if (typeof(TChild).GetProperty(childPropertyName) != null)
                ChildProperty = typeof(TChild).GetProperty(childPropertyName);
        }

        /// <summary>
        /// Checks the text for matches
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<TChild> CheckForMatches(string text)
        {
            // create a list of child objects
            List<TChild> childObjs = new List<TChild>();

            if (ParentProperty != null)
            {
                // get parent object matches
                List<TParent> parentObjs = ParentObjectMatcher.GetMatches(text);

                // if there were any parent matches, get child objects
                if (parentObjs != null && parentObjs.Count > 0)
                {
                    // for each parent, check for children
                    foreach (TParent parentObj in parentObjs)
                    {
                        // get the value of the parent linking property
                        object parentPropertyValue = ParentProperty.GetValue(parentObj, null);

                        // if the child property is set
                        if (ChildProperty != null)
                        {
                            // find child object matches
                            List<TChild> childObjMatches = ChildObjectMatcher.GetMatches(text);

                            // if any child matches were found, add them to the collection
                            if (childObjMatches != null && childObjMatches.Count > 0)
                            {
                                // for each child match, set the child's parent property and add to the collection
                                foreach (TChild childObj in childObjMatches)
                                {
                                    ChildProperty.SetValue(childObj, parentPropertyValue, null);
                                    childObjs.Add(childObj);
                                }
                            }
                        }
                    }
                }
            }

            return childObjs;
        }

        #endregion
    }
}
