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
using System.ComponentModel;
using System.Windows.Ria.Data;
using System.Windows.Data;

using Microsoft.Practices.Composite.Events;

using Prism.Samples.Web.OrderServices;
using Prism.Samples.Web.OrderServices.DataModel;

namespace Prism.Samples.Module1.ViewModels
{
    /// <summary>
    /// ViewModel for the OrderList view.
    /// </summary>
    public class OrderListViewModel : INotifyPropertyChanged
    {
        private readonly OrderContext     _orderService;
        private readonly IEventAggregator _eventAggregator;

        private PagedCollectionView _orderCollectionView;

        public OrderListViewModel( OrderContext orderService, IEventAggregator eventAggregator )
        {
            // Event aggregator for publishing loosely coupled events.
            _eventAggregator = eventAggregator;

            // Order service context for loading Order data from web service.
            _orderService = orderService;

            // Wrap the order entity list in a PagedCollectionView.
            _orderCollectionView = new PagedCollectionView( _orderService.Orders );

            // Track the currently selected item.
            _orderCollectionView.CurrentChanged += SelectedOrderChanged;

            // Start loading the orders collection using the order service.
            LoadOperation load = _orderService.Load( _orderService.GetOrdersQuery(), OnOrdersLoaded, null );
        }

        public ICollectionView Orders
        {
            get { return _orderCollectionView; }
        }

        private void OnOrdersLoaded( LoadOperation<Order> operation )
        {
            // When the data is loaded, raise the property changed event for the Orders property.
            if ( !operation.HasError ) NotifyPropertyChanged( "Orders" );

            // TODO: Check for and handle any load errors...
        }

        private void SelectedOrderChanged( object sender, EventArgs e )
        {
            // Publish the OrderSelectedEvent event.
            Order selectedOrder = _orderCollectionView.CurrentItem as Order;
            if ( selectedOrder != null )
            {
                _eventAggregator.GetEvent<OrderSelectedEvent>().Publish( selectedOrder );
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged( string propertyName )
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }
        #endregion
    }
}
