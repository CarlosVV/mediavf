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
using System.Windows.Data;

using Microsoft.Practices.Composite.Events;

using Prism.Samples.Common;
using Prism.Samples.Web.OrderServices.DataModel;

namespace Prism.Samples.Module1.ViewModels
{
    /// <summary>
    /// ViewModel for the OrderDetailsView.
    /// </summary>
    public class OrderDetailsViewModel: INotifyPropertyChanged
    {
        private readonly IEventAggregator _eventAggregator;

        private PagedCollectionView _orderDetailsCollectionView;

        public OrderDetailsViewModel( IEventAggregator eventAggregator )
        {
            // Event aggregator for subscribing to loosely coupled events.
            _eventAggregator = eventAggregator;

            // Subscribe to the OrderSelectedEvent event.
            _eventAggregator.GetEvent<OrderSelectedEvent>().Subscribe( OrderSelected, true );
        }

        public ICollectionView OrderDetails
        {
            get { return _orderDetailsCollectionView; }
        }

        private void OrderSelected( Order currentOrder )
        {
            if ( _orderDetailsCollectionView != null )
            {
                _orderDetailsCollectionView.CurrentChanged -= SelectedOrderDetailChanged;
            }

            // Wrap the order entity list in a PagedCollectionView.
            _orderDetailsCollectionView = new PagedCollectionView( currentOrder.Order_Details );

            // Track the currently selected item.
            _orderDetailsCollectionView.CurrentChanged += SelectedOrderDetailChanged;

            // When we get new Order Details, raise the property changed event for the OrderDetails property.
            NotifyPropertyChanged( "OrderDetails" );

            // Hmmm. The paged collection view seems to reset the currently selected item
            // to the first item in the collection but it doesn't fire a CurrentChanged event.
            // Have to manually update the call the event handler.
            SelectedOrderDetailChanged( this, new EventArgs() );
        }

        private void SelectedOrderDetailChanged( object sender, EventArgs e )
        {
            // Publish the OrderDetailSelectedEvent event.
            Order_Detail selectedOrderDetail = _orderDetailsCollectionView.CurrentItem as Order_Detail;
            if ( selectedOrderDetail != null )
            {
                _eventAggregator.GetEvent<ProductSelectedEvent>().Publish( selectedOrderDetail.ProductID );
            }
            else
            {
                _eventAggregator.GetEvent<ProductSelectedEvent>().Publish( -1 );
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
