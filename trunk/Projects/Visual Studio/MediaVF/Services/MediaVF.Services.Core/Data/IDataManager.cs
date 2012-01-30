using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using MediaVF.Services.Core.Components;
using MediaVF.Services.Core.Configuration;
using System.Collections;

namespace MediaVF.Services.Core.Data
{
    public interface IDataManager
    {
        void AddDataContext(DataContextElement contextConfig);

        DataContext GetDataContext<T>();

        #region Shared Cached Data

        bool IsSharedCachedData<T>();

        bool IsSharedCachedData(Type t);

        IList GetSharedCachedData(Type t);

        List<T> GetSharedCachedData<T>();

        #endregion
    }
}
