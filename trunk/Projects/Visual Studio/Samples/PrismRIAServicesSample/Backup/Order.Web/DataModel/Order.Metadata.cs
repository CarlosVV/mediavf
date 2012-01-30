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
using System.ComponentModel.DataAnnotations;
using System.Web.DomainServices;
using System.Data.Objects.DataClasses;

namespace Prism.Samples.Web.OrderServices.DataModel
{
    [MetadataType( typeof( OrderMetadata ) )]
    public partial class Order
    {
        internal sealed class OrderMetadata
        {
            [Key]
            public int OrderID;

            [Include]
            public EntityCollection<Order_Detail> Order_Details;
        }
    }
}
