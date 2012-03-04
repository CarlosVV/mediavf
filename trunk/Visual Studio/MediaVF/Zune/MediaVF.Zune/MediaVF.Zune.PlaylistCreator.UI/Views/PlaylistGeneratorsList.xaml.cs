using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaVF.Zune.PlaylistCreator.UI.ViewModels;

namespace MediaVF.Zune.PlaylistCreator.UI.Views
{
    /// <summary>
    /// Interaction logic for PlaylistGeneratorsList.xaml
    /// </summary>
    public partial class PlaylistGeneratorsList : UserControl
    {
        public PlaylistGeneratorsList()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((PlaylistGeneratorsListViewModel)DataContext).OpenGenerators();
        }
    }
}
