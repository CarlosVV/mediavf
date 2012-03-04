using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Services.Components;
using MediaVF.Services.Data;
using System.Reflection;

namespace MediaVF.Services.Polling.Matching
{
    public class PropertyValueMatcher<T> : ITextMatcher<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the service component to which this matcher belongs
        /// </summary>
        IServiceComponent ServiceComponent { get; set; }

        /// <summary>
        /// Gets or sets a function for retrieving the objects
        /// </summary>
        Func<IEnumerable<T>> ObjectProvider { get; set; }

        /// <summary>
        /// Gets or sets the name of the property on the objects that will be used to find matches
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Gets the property on the type using the property name
        /// </summary>
        PropertyInfo _property;
        PropertyInfo Property
        {
            get
            {
                if (_property == null && !string.IsNullOrEmpty(PropertyName))
                    _property = typeof(T).GetProperty(PropertyName);
                return _property;
            }
        }

        /// <summary>
        /// Gets a dictionary mapping property values to objects
        /// </summary>
        Dictionary<string, T> _objectsByPropertyValue;
        Dictionary<string, T> ObjectsByPropertyValue
        {
            get
            {
                if (_objectsByPropertyValue == null)
                    _objectsByPropertyValue = new Dictionary<string, T>();
                return _objectsByPropertyValue;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a property value matcher
        /// </summary>
        /// <param name="serviceComponent">The service component to which this matcher belongs</param>
        /// <param name="objectProvider">The function used to provide the objects to use for matching</param>
        /// <param name="propertyName">The name of the property on the objects to be used for matching</param>
        public PropertyValueMatcher(IServiceComponent serviceComponent, Func<IEnumerable<T>> objectProvider, string propertyName)
        {
            ServiceComponent = serviceComponent;
            ObjectProvider = objectProvider;
            PropertyName = propertyName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the matcher by getting all objects used for matching and mapping them using their properties
        /// </summary>
        public void Initialize()
        {
            // get a list of objects to use to look for matches
            IEnumerable<T> objectList = null;
            if (ObjectProvider != null)
                objectList = ObjectProvider();

            //if (DataManager.IsSharedCachedData<T>())
            //    objectList = DataManager.GetSharedCachedData<T>();
            //else
            //    objectList = DataManager.GetDataContext<T>().GetByModuleID<T>(ServiceComponent.ID);

            // if any objects were provided and a property for matching is set
            if (objectList != null && Property != null)
            {
                // map each object using the value of its property
                foreach (T obj in objectList)
                {
                    // get the property value as text
                    string propertyText = null;
                    object propertyValue = Property.GetValue(obj, null);
                    if (propertyValue != null)
                        propertyText = propertyValue.ToString();

                    // store the object mapped to the text value of its property
                    if (propertyText != null)
                        ObjectsByPropertyValue.Add(propertyText, obj);
                }
            }
        }

        /// <summary>
        /// Gets matches where the text value of the property on the object is contained within the given text
        /// </summary>
        /// <param name="text">The text to check for matches</param>
        /// <returns>A list of matching objects</returns>
        public List<T> GetMatches(string text)
        {
            // return the objects whose property's text value is contained within the given text
            return ObjectsByPropertyValue.Keys
                .Where(propertyValue =>
                    !string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(propertyValue) && text.ToLower().Contains(propertyValue.ToLower()))
                .Select(propertyValue =>
                    ObjectsByPropertyValue[propertyValue]).ToList();
        }

        #endregion
    }
}
