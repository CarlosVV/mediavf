using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Services.Core.Components;
using MediaVF.Services.Core.Data;
using System.Reflection;

namespace MediaVF.Services.Polling.Matching
{
    public class PropertyValueMatcher<T> : ITextMatcher<T>
    {
        IServiceComponent ServiceComponent { get; set; }

        IDataManager DataManager { get; set; }

        string PropertyName { get; set; }

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

        public PropertyValueMatcher(IServiceComponent serviceComponent, IDataManager dataManager, string propertyName)
        {
            ServiceComponent = serviceComponent;
            DataManager = dataManager;

            PropertyName = propertyName;
        }

        public void Initialize()
        {
            List<T> objectList = null;
            if (DataManager.IsSharedCachedData<T>())
                objectList = DataManager.GetSharedCachedData<T>();
            else
                objectList = DataManager.GetDataContext<T>().GetByModuleID<T>(ServiceComponent.ID);

            if (objectList != null && Property != null)
                objectList.ForEach(obj =>
                {
                    string propertyText = null;
                    object propertyValue = Property.GetValue(obj, null);
                    if (propertyValue != null)
                        propertyText = propertyValue.ToString();

                    if (propertyText != null)
                        ObjectsByPropertyValue.Add(propertyText, obj);
                });
        }

        public List<T> GetMatches(string text)
        {
            return ObjectsByPropertyValue.Keys
                .Where(propertyValue =>
                    !string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(propertyValue) && text.ToLower().Contains(propertyValue.ToLower()))
                .Select(propertyValue =>
                    ObjectsByPropertyValue[propertyValue]).ToList();
        }
    }
}
