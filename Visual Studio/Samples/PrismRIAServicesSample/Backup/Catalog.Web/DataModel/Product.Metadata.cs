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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Ria.Data;
using System.Web.DomainServices;

namespace Prism.Samples.Web.CatalogServices.DataModel
{
    [MetadataType( typeof( ProductMetadata ) )]
    public partial class Product
    {
        internal sealed class ProductMetadata
        {
            [Key]
            public int ProductID;

            [Include]
            public Category Categories;
        }
    }
}
