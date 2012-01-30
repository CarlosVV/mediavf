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
using System.Linq;
using System.ComponentModel;
using System.Windows.Ria.Data;

using Microsoft.Practices.Composite.Events;

using Prism.Samples.Common;
using Prism.Samples.Web.CatalogServices;
using Prism.Samples.Web.CatalogServices.DataModel;

namespace Prism.Samples.Module2.ViewModels
{
    /// <summary>
    /// ViewModel for the Product Detail view.
    /// </summary>
    public class ProductDetailViewModel : INotifyPropertyChanged
    {
        private readonly CatalogContext   _catalogService;
        private readonly IEventAggregator _eventAggregator;

        public ProductDetailViewModel( CatalogContext catalogService, IEventAggregator eventAggregator )
        {
            // Event aggregator for publishing loosely coupled events.
            _eventAggregator = eventAggregator;

            // Catalog service context for loading Catalog data from web service.
            _catalogService = catalogService;

            // Subscribe to the OrderDetailSelectedEvent event.
            _eventAggregator.GetEvent<ProductSelectedEvent>().Subscribe( LineItemSelected, true );
        }

        private void LineItemSelected( int productId )
        {
            if ( productId == -1 )
            {
                CurrentProduct = null;
                return;
            }

            // Start loading the product information using the catalog service.
            LoadOperation load = _catalogService.Load( _catalogService.GetProductByIdQuery( productId ) );
            load.Completed += ( sender, args ) =>
            {
                CurrentProduct = _catalogService.Products.FirstOrDefault( item => item.ProductID == productId );
            };
        }

        private Product _product;
        public Product CurrentProduct
        {
            get { return _product; }
            set
            {
                _product = value;
                NotifyPropertyChanged( "CurrentProduct" );
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
