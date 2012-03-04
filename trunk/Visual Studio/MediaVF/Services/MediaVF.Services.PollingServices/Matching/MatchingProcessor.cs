using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Unity;

using MediaVF.Common.Entities;

using MediaVF.Services.Core.Data;
using MediaVF.Services.Core.Components;
using MediaVF.Services.Polling.Content;
using MediaVF.Services.Polling.Processing;

namespace MediaVF.Services.Polling.Matching
{
    [ContentProcessor(typeof(TextContent))]
    [ContentProcessor(typeof(HtmlContent))]
    [ContentProcessor(typeof(XmlContent))]
    public class ParentChildMatchingProcessor<TParent, TChild> : ContentProcessor
    {
        public string Name { get; private set; }
        
        IServiceComponent ServiceComponent { get; set; }

        IDataManager DataManager { get; set; }

        ITextMatcher<TParent> ParentObjectMatcher { get; set; }

        ITextMatcher<TChild> ChildObjectMatcher { get; set; }

        PropertyInfo ParentProperty { get; set; }

        PropertyInfo ChildProperty { get; set; }

        public ParentChildMatchingProcessor(IServiceComponent serviceComponent, IDataManager dataManager, string name)
        {
            ServiceComponent = serviceComponent;
            DataManager = dataManager;
            Name = name;
        }

        public override void Initialize()
        {
            ParentObjectMatcher = ServiceComponent.ComponentContainer.Resolve<ITextMatcher<TParent>>(Name);
            if (ParentObjectMatcher != null)
                ParentObjectMatcher.Initialize();

            ChildObjectMatcher = ServiceComponent.ComponentContainer.Resolve<ITextMatcher<TChild>>(Name);
            if (ChildObjectMatcher != null)
                ChildObjectMatcher.Initialize();
        }

        protected override void ProcessItem(IContent content)
        {
            Type typeOfContent = content.GetType();

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

                DataContext context = DataManager.GetDataContext<TParent>();
                context.AddObjects(childObjs);

                context.Save();
            }
        }

        public void SetParentChildLink(string parentPropertyName, string childPropertyName)
        {
            if (typeof(TParent).GetProperty(parentPropertyName) != null)
                ParentProperty = typeof(TParent).GetProperty(parentPropertyName);

            if (typeof(TChild).GetProperty(childPropertyName) != null)
                ChildProperty = typeof(TChild).GetProperty(childPropertyName);
        }

        private List<TChild> CheckForMatches(string text)
        {
            List<TChild> childObjs = new List<TChild>();

            if (ParentProperty != null)
            {
                List<TParent> parentObjs = ParentObjectMatcher.GetMatches(text);

                if (parentObjs != null && parentObjs.Count > 0)
                {
                    foreach (TParent parentObj in parentObjs)
                    {
                        object parentPropertyValue = ParentProperty.GetValue(parentObj, null);

                        if (ChildProperty != null)
                        {
                            List<TChild> childObjMatches = ChildObjectMatcher.GetMatches(text);

                            if (childObjMatches != null && childObjMatches.Count > 0)
                            {
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
    }
}
