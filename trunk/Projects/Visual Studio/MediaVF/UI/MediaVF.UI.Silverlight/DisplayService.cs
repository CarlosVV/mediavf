using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;

namespace MediaVF.UI.Core
{
    public class DisplayService : IDisplayService
    {
        public IUnityContainer Container { get; private set; }

        public DisplayService(IUnityContainer container)
        {
            Container = container;
        }

        public void Display(ViewModelBase viewModel)
        {
            if (viewModel != null)
            {
                string viewTypeText = Application.Current.Resources[viewModel.GetType().FullName] as string;
                if (!string.IsNullOrEmpty(viewTypeText))
                {
                    Type viewType = Type.GetType(viewTypeText);
                    if (viewType != null)
                    {
                        if (typeof(ChildWindow).IsAssignableFrom(viewType))
                        {
                            ChildWindow childWindow = Container.Resolve(viewType) as ChildWindow;
                            childWindow.DataContext = viewModel;
                            childWindow.VerticalAlignment = VerticalAlignment.Center;
                            if (viewModel is ClosableViewModel)
                                ((ClosableViewModel)viewModel).Close += () => childWindow.Close();
                            childWindow.Show();
                        }
                        else
                        {
                            IRegionManager regionManager = Container.Resolve<IRegionManager>();
                            IRegion region = regionManager.Regions.FirstOrDefault(r => r.Views.Any(v => v != null && v.GetType() == viewType));
                            if (region != null)
                            {
                                object obj = region.Views.First(v => v != null && v.GetType() == viewType);
                                if (obj is FrameworkElement)
                                {
                                    ((FrameworkElement)obj).DataContext = viewModel;
                                    region.Activate(obj);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Display<T>() where T : ViewModelBase
        {
            T viewModel = Container.Resolve<T>();

            Display(viewModel);
        }
    }
}
