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

using Prism.Samples.Web.CatalogServices;
using Prism.Samples.Common;
using Prism.Samples.Module2.Views;

namespace Prism.Samples.Module2
{
    public class ModuleInit : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public ModuleInit( IUnityContainer container, IRegionManager regionManager )
        {
            _container     = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            // Register the CatalogContext type with the container.
            // New instances will be constructed using the default constructor.
            _container.RegisterType<CatalogContext>( new InjectionConstructor() );

            // Show the ProductDetailView in the Shell's bottom left hand region.
            _regionManager.RegisterViewWithRegion( RegionNames.BottomRightRegion, () => this._container.Resolve<ProductDetailView>() );
        }

        #endregion
    }
}
