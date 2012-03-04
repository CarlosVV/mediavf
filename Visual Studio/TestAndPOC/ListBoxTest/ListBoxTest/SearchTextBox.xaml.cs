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
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace ListBoxTest
{
    public partial class SearchTextBox : UserControl
    {
        #region Dependency Properties

        #region DisplayMemberPath

        /// <summary>
        /// Dependency property for the DisplayMemberPath
        /// </summary>
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath",
            typeof(string),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the path to the property used for display in the list box
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        #endregion

        #region ItemsSource

        /// <summary>
        /// Dependency property for MatchingItems
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a collection of matching items based on the current text in the textbox
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion

        #region MaxDisplayItemsCount

        /// <summary>
        /// Dependency property for MaxDisplayItemsCount
        /// </summary>
        public static readonly DependencyProperty MaxDisplayItemsCountProperty = DependencyProperty.Register("MaxDisplayItemsCount",
            typeof(int),
            typeof(SearchTextBox),
            new PropertyMetadata(20));

        /// <summary>
        /// Gets or sets the max display item count
        /// </summary>
        public int MaxDisplayItemsCount
        {
            get { return (int)GetValue(MaxDisplayItemsCountProperty); }
            set { SetValue(MaxDisplayItemsCountProperty, value); }
        }

        #endregion

        #region ItemContainerStyle

        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle",
            typeof(Style),
            typeof(SearchTextBox),
            new PropertyMetadata(null, OnItemContainerStylePropertyChanged));

        private static void OnItemContainerStylePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SearchTextBox searchTextBox = (SearchTextBox)sender;
            searchTextBox.MatchingItemsListBox.ItemContainerStyle = (Style)args.NewValue;
        }

        #endregion

        #region ItemsPanelTemplate

        public static readonly DependencyProperty ItemsPanelTemplateProperty = DependencyProperty.Register("ItemsPanelTemplate",
            typeof(ItemsPanelTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null, OnItemsPanelTemplatePropertyChanged));

        private static void OnItemsPanelTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SearchTextBox searchTextBox = (SearchTextBox)sender;
            searchTextBox.MatchingItemsListBox.ItemsPanel = (ItemsPanelTemplate)args.NewValue;
        }

        #endregion

        #region ItemTemplate

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate",
            typeof(DataTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null, OnItemTemplatePropertyChanged));

        private static void OnItemTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SearchTextBox searchTextBox = (SearchTextBox)sender;
            searchTextBox.MatchingItemsListBox.ItemTemplate = (DataTemplate)args.NewValue;
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current search worker
        /// </summary>
        BackgroundWorker CurrentWorker { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a search textbox
        /// </summary>
        public SearchTextBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handle pressing of the down key to set focus on the dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // handle pressing the down key
            if (e.Key == Key.Down && ItemsSource != null && ItemsSource.Cast<object>().Any())
            {
                MatchingItemsListBox.Focus();
                MatchingItemsListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Handles changes to the textbox by showing/hiding the dropdown and/or filtering its items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                // set dropdown width and offset and open it
                MatchingItemsDropdown.Width = InputTextBox.ActualWidth;
                MatchingItemsDropdown.VerticalOffset = InputTextBox.ActualHeight + 1;
                MatchingItemsDropdown.IsOpen = true;

                // set visibility
                SearchingTextBlock.Visibility = Visibility.Visible;
                SearchingProgressBar.Visibility = Visibility.Visible;
                MatchingItemsListBox.Visibility = Visibility.Collapsed;

                // if there are any bound items, filter them
                if (ItemsSource != null && ItemsSource.Cast<object>().Any())
                {
                    if (CurrentWorker != null)
                        CurrentWorker.CancelAsync();

                    CurrentWorker = GetSearchWorker();
                    CurrentWorker.RunWorkerAsync(new object[]
                    {
                        ItemsSource.Cast<object>(),
                        DisplayMemberPath,
                        MaxDisplayItemsCount,
                        InputTextBox.Text
                    });
                }
            }
            else
                MatchingItemsDropdown.IsOpen = false;
        }

        #endregion

        #region Search

        /// <summary>
        /// Gets a new search worker
        /// </summary>
        /// <returns></returns>
        private BackgroundWorker GetSearchWorker()
        {
            // create search worker
            BackgroundWorker searchWorker = new BackgroundWorker();

            // worker must be cancelable
            searchWorker.WorkerSupportsCancellation = true;

            // attach to worker events
            searchWorker.DoWork += new DoWorkEventHandler(OnSearch);
            searchWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnSearchCompleted);

            return searchWorker;
        }

        /// <summary>
        /// Runs a search on a background thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSearch(object sender, DoWorkEventArgs e)
        {
            // get search text arg
            IEnumerable<object> items = (IEnumerable<object>)((object[])e.Argument)[0];
            string displayMemberPath = (string)((object[])e.Argument)[1];
            int maxDisplayItemsCount = (int)((object[])e.Argument)[2];
            string searchText = (string)((object[])e.Argument)[3];

            // keep list of matching items
            List<object> matchingItems = new List<object>();

            if (items != null)
            {
                foreach (object o in items)
                {
                    // if search has been canceled just stop
                    if (e.Cancel || matchingItems.Count >= maxDisplayItemsCount)
                        break;

                    if (o != null)
                    {
                        // get display property
                        if (!string.IsNullOrEmpty(displayMemberPath))
                        {
                            PropertyInfo displayProperty = o.GetType().GetProperty(displayMemberPath);
                            if (displayProperty != null)
                            {
                                // get display property value and check if it contains text
                                object displayPropValue = displayProperty.GetValue(o, null);
                                if (displayPropValue != null &&
                                    displayPropValue.ToString().ToLower().StartsWith(searchText.ToLower()))
                                    matchingItems.Add(o);
                            }
                        }
                        else if (o.ToString().ToLower().StartsWith(searchText.ToLower()))
                            matchingItems.Add(o);
                    }
                }
            }

            e.Result = matchingItems;
        }

        /// <summary>
        /// Handles completion of search by displaying results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                // set results
                IEnumerable results = (IEnumerable)e.Result;
                MatchingItemsListBox.ItemsSource = results;

                // set visibility
                SearchingTextBlock.Visibility = Visibility.Collapsed;
                SearchingProgressBar.Visibility = Visibility.Collapsed;
                MatchingItemsListBox.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}
