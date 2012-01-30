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
using System.Web.Ria;
using System.Web.DomainServices.LinqToEntities;

using Prism.Samples.Web.OrderServices.DataModel;

namespace Prism.Samples.Web.OrderServices
{
    [EnableClientAccess()]
    public class OrderService : LinqToEntitiesDomainService<NorthwindOrders>
    {
        public IQueryable<Order> GetOrders()
        {
            // Include the order details in the order query.
            return this.Context.OrderSet.Include( "Order_Details" ).Include( "Order_Details.Products" );
        }

        public IQueryable<Order_Detail> GetDetailsForOrder( int orderID )
        {
            // Include the product name in the order detail query.
            return this.Context.Order_DetailSet.Include( "Products" ).Where( orderDetail => orderDetail.OrderID == orderID );
        }
    }
}


