using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using MediaVF.Services.Core.Logging;

namespace MediaVF.Services.Polling.Content
{
    public static class ContentFactory
    {
        public static TextContent GetTextContent<T>(IUnityContainer container, T item)
        {
            IContentAccessor<T> contentAccessor = CreateContentAccessor<T>(container, item);

            if (contentAccessor != null && contentAccessor.RawContentIsPlainText)
            {
                contentAccessor.Load(item);

                TextContent textContent = new TextContent();
                textContent.Load(contentAccessor.RawContent as string);
                return textContent;
            }
            else
                return null;
        }

        public static IContent GetContent<T>(IUnityContainer container, T item)
        {
            IContentAccessor<T> contentAccessor = CreateContentAccessor<T>(container, item);

            if (contentAccessor != null)
            {
                contentAccessor.Load(item);

                return CreateContent(contentAccessor);
            }
            else
                return null;
        }

        static IContentAccessor<T> CreateContentAccessor<T>(IUnityContainer container, T item)
        {
            // get registered accessors
            IEnumerable<IContentAccessor<T>> contentAccessors = container.ResolveAll<IContentAccessor<T>>();

            // look for a match
            IContentAccessor<T> matchedAccessor = null;
            if (contentAccessors != null)
                matchedAccessor =
                    contentAccessors.FirstOrDefault(contentAccessor =>
                    {
                        // check valid content
                        if (contentAccessor.HasValidContent(item))
                            return true;

                        return false;
                    });

            // return a match if found
            return matchedAccessor;
        }
        
        static IContent CreateContent(IContentAccessor accessor)
        {
            // match content to accessors with the content type attribute
            Assembly contentAssembly = Assembly.GetAssembly(typeof(IContent));
            List<Type> contentObjTypes = contentAssembly.GetTypes()
                .Where(type => typeof(IContent).IsAssignableFrom(type))
                .Where(contentObjType =>
                    contentObjType.GetCustomAttributes(typeof(ContentTypeAttribute), true)
                    .Where(attribute => string.Compare(((ContentTypeAttribute)attribute).ContentType, accessor.ContentType, true) == 0).Count() > 0)
                .ToList();

            if (contentObjTypes.Count != 0)
                throw new NoContentHandlerFoundException();

            IContent contentObj = (IContent)Activator.CreateInstance(contentObjTypes[0]);
            try
            {
                contentObj.Load(accessor.RawContent);
            }
            catch (Exception ex)
            {
                contentObj.IsLoaded = false;

                ServiceLocator.Current.GetInstance<IComboLog>().Error(string.Format("Failed to load content for content type {0}.", accessor.ContentType), ex);

                throw new InvalidContentException();
            }

            return contentObj;
        }
    }
}
