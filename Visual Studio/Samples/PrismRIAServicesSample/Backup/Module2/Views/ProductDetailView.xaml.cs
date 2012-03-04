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
using System.Windows.Controls;
using System.Windows.Media.Animation;

using Prism.Samples.Module2.ViewModels;

namespace Prism.Samples.Module2.Views
{
    public partial class ProductDetailView : UserControl
    {
        private Storyboard imageAnimation;

        public ProductDetailView( ProductDetailViewModel viewModel )
        {
            InitializeComponent();

            // Set the ViewModel as this View's data context.
            this.DataContext = viewModel;

            // Get a reference to the product image animation.
            imageAnimation = this.Resources[ "ProductImageStoryboard" ] as Storyboard;

            // Animate the product image when the current product changes.
            viewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler( CurrentProduct_PropertyChanged );
        }

        private void CurrentProduct_PropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            // This is an example of UI logic - logic that drives the cosmetic behavior
            // of the application and is not core to the application's presentation or
            // business logic. As such this logic does not belong in the ViewModel.
            // Even so, it really should be encapsulated in a behavior so that it can be
            // applied declaratively in XAML. Will leave to a future post...
            if ( e.PropertyName == "CurrentProduct" )
            {
                imageAnimation.Stop();
                imageAnimation.Begin();
            }
        }
    }
}
