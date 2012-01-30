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

namespace Prism.Samples.Web.CatalogServices.DataModel
{
    public partial class Product
    {
        /// <summary>
        /// Extend the Product class to introduce a client-side property to
        /// return a local Uri to the item's image.
        /// Not a very good example since you'd never embed images in a
        /// client side assembly like this - just meant to illustrate how
        /// to extend an entity with (client-side only) additional properties
        /// and methods.
        /// </summary>
        public string ProductImageUrl
        {
            get
            {
                int imageID = this.ProductID % 10;
                return string.Format( "../Images/{0}.JPG", Convert.ToString( imageID ) );
            }
        }
    }
}
