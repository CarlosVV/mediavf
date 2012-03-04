///////////////////////////////////////////////////////////////////////////////
//
// Copyright (C) 2008-2009 David Hill. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

using Prism.Samples.Common;
using Prism.Samples.Module1.Views;
using Prism.Samples.Web.OrderServices;

namespace Prism.Samples.Module1
{
    public class ModuleInit : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager  _regionManager;

        public ModuleInit( IUnityContainer container, IRegionManager regionManager )
        {
            _container     = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            // Register the OrderContext type with the container.
            // New instances will be constructed using the default constructor.
            _container.RegisterType<OrderContext>( new InjectionConstructor() );

            // Show the OrderListView in the Shell's left hand region.
            _regionManager.RegisterViewWithRegion( RegionNames.LeftRegion, () => _container.Resolve<OrderListView>() );

            // Show the OrderDetailsView in the Shell's top right hand region.
            _regionManager.RegisterViewWithRegion( RegionNames.TopRightRegion, () => _container.Resolve<OrderDetailsView>() );
        }

        #endregion
    }
}
