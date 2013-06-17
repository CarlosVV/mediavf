using System.Windows;

namespace AutoTrade.Core.UI.Controls
{
    /// <summary>
    /// Interaction logic for StockSearchControl.xaml
    /// </summary>
    public partial class StockSearchControl
    {
        public StockSearchControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption",
                typeof(string),
                typeof(StockSearchControl),
                new PropertyMetadata("Search:", CaptionPropertyChangedCallback));

        private static void CaptionPropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var searchableComboBox = dependencyObject as StockSearchControl;

            if (searchableComboBox != null)
                searchableComboBox.CaptionTextBlock.Text = (string)dependencyPropertyChangedEventArgs.NewValue;
        }

        public string Caption
        {
            get { return (string) GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }
    }
}
