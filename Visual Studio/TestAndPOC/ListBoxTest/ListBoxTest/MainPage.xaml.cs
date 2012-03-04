using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ListBoxTest
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            /*
            TestCollectionViewModel testVM = new TestCollectionViewModel();
            testVM.Items.Add(new TestItemViewModel() { Name = "Astomatous" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Belphegor" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Carnifex" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Dead to Fall" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Eibon" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Fall of Troy" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Gorguts" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Heaven Shall Burn" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Ion Dissonance" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Job for a Cowboy" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Kalmah" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Landtlos" });
            testVM.Items.Add(new TestItemViewModel() { Name = "Mercenary" });
            */

            DataContext = TestCollectionViewModel.Get(1000, 10);
        }
    }
}
