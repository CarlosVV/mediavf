using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Configuration
{
    public interface IServiceConfigManager
    {
        void Load();

        int ApplicationID { get; }

        ServiceConfigSection Configuration { get; }

        string ComponentDBConnectionString { get; }

        string GetComponentSetting(Type componentType, string key);

        Dictionary<Type, ServiceComponentElement> Components { get; }
    }
}
