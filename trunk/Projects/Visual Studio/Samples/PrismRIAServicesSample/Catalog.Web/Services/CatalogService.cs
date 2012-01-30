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

using Prism.Samples.Web.CatalogServices.DataModel;

namespace Prism.Samples.Web.CatalogServices
{
    [EnableClientAccess()]
    public class CatalogService : LinqToEntitiesDomainService<NorthwindEntities>
    {
        public IQueryable<Category> GetCategories()
        {
            return this.Context.CategorySet;
        }

        public IQueryable<Product> GetAllProducts()
        {
            // Include the category in the product query.
            return this.Context.ProductSet.Include( "Categories" );
        }

        public IQueryable<Product> GetProductById( int productId )
        {
            // Include the category in the product query.
            return this.Context.ProductSet.Include( "Categories" ).Where( product => product.ProductID == productId );
        }
    }
}


