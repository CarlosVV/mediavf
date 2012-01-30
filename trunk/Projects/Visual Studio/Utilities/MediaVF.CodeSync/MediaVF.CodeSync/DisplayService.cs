using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaVF.UI.Core;
using Microsoft.Practices.Unity;
using System.Windows;

namespace MediaVF.CodeSync
{
    class DisplayService : IDisplayService
    {
        /// <summary>
        /// Gets the unity container
        /// </summary>
        public IUnityContainer Container { get; private set; }

        /// <summary>
        /// Instantiate display service
        /// </summary>
        /// <param name="container"></param>
        public DisplayService(IUnityContainer container)
        {
            Container = container;
        }
        
        /// <summary>
        /// Creates a view model and displays it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Display<T>() where T : ViewModelBase
        {
            // create view model
            T viewModel = Container.Resolve<T>();

            // display the view model
            Display(viewModel);
        }

        /// <summary>
        /// Displays a view model
        /// </summary>
        /// <param name="viewModelBase"></param>
        public void Display(ViewModelBase viewModelBase)
        {
            if (viewModelBase != null)
            {
                // get the data template for the type
                DataTemplate dataTemplate = (DataTemplate)App.Current.TryFindResource(new DataTemplateKey(viewModelBase.GetType()));
                if (dataTemplate != null)
                {
                    // load the template
                    DependencyObject obj = dataTemplate.LoadContent();

                    if (obj is FrameworkElement)
                    {
                        // set data context
                        FrameworkElement element = (FrameworkElement)obj;
                        element.DataContext = viewModelBase;

                        if (element is Window)
                        {
                            // attach to close event
                            Window window = (Window)element;
                            if (viewModelBase is ClosableViewModel)
                                ((ClosableViewModel)viewModelBase).Close += window.Close;

                            // show window
                            if (viewModelBase is WindowViewModel && ((WindowViewModel)viewModelBase).Modal)
                                window.ShowDialog();
                            else
                                window.Show();
                        }
                    }
                }
            }
        }
    }
}
