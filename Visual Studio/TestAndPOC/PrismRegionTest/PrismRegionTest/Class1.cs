using System;
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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Regions;



namespace PrismRegionTest
{
    public class Class1
    {
        public void Test()
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            IRegionCollection regions = container.Resolve<IRegionManager>().Regions;
        }
    }
}
