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
using System.Windows.Controls.Primitives;

namespace MediaVF.UI.Controls
{
    public partial class SearchTextBox : UserControl
    {
        #region Dependency Properties

        #region SearchMemberPath

        /// <summary>
        /// Dependency property for SearchMemberPath
        /// </summary>
        public static readonly DependencyProperty SearchMemberPathProperty = DependencyProperty.Register("SearchMemberPath",
            typeof(string),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the path to the property against which to search
        /// </summary>
        public string SearchMemberPath
        {
            get { return (string)GetValue(SearchMemberPathProperty); }
            set { SetValue(SearchMemberPathProperty, value); }
        }

        #endregion

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

        #region SelectedItem

        /// <summary>
        /// Dependency property for SelectedItem
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem",
            typeof(object),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the selected item in the dropdown
        /// </summary>
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
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
            new PropertyMetadata(null));

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        #endregion

        #region ItemsPanelTemplate

        public static readonly DependencyProperty ItemsPanelTemplateProperty = DependencyProperty.Register("ItemsPanelTemplate",
            typeof(ItemsPanelTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public ItemsPanelTemplate ItemsPanel
        {
            get { return (ItemsPanelTemplate)GetValue(ItemsPanelTemplateProperty); }
            set { SetValue(ItemsPanelTemplateProperty, value); }
        }

        #endregion

        #region ItemTemplate

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate",
            typeof(DataTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the item template
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        #endregion

        #region ResultsTemplate

        public static readonly DependencyProperty ResultsTemplateProperty = DependencyProperty.Register("ResultsTemplate",
            typeof(DataTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null));

        public DataTemplate ResultsTemplate
        {
            get { return (DataTemplate)GetValue(ResultsTemplateProperty); }
            set { SetValue(ResultsTemplateProperty, value); }
        }

        #endregion

        #region SearchingMessage

        public static readonly DependencyProperty SearchingMessageProperty = DependencyProperty.Register("SearchingMessage",
            typeof(string),
            typeof(SearchTextBox),
            new PropertyMetadata("Searching...", new PropertyChangedCallback(OnSearchingMessagePropertyChanged)));

        private static void OnSearchingMessagePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SearchTextBox searchTextBox = (SearchTextBox)sender;
            if (!string.IsNullOrEmpty((string)args.NewValue) && searchTextBox.SearchingTemplate != null)
                throw new Exception("Cannot set both SearchingMessage and SearchingTemplate.");
        }

        public string SearchingMessage
        {
            get { return (string)GetValue(SearchingMessageProperty); }
            set { SetValue(SearchingMessageProperty, value); }
        }

        #endregion

        #region SearchingTemplate

        public static readonly DependencyProperty SearchingTemplateProperty = DependencyProperty.Register("SearchingTemplate",
            typeof(DataTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSearchingTemplatePropertyChanged)));

        private static void OnSearchingTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SearchTextBox searchTextBox = (SearchTextBox)sender;
            if (!string.IsNullOrEmpty(searchTextBox.SearchingMessage) && args.NewValue != null)
                throw new Exception("Cannot set both SearchingMessage and SearchingTemplate.");
        }

        public DataTemplate SearchingTemplate
        {
            get { return (DataTemplate)GetValue(SearchingTemplateProperty); }
            set { SetValue(SearchingTemplateProperty, value); }
        }

        #endregion

        #region NoResultsMessage

        public static readonly DependencyProperty NoResultsMessageProperty = DependencyProperty.Register("NoResultsMessage",
            typeof(string),
            typeof(SearchTextBox),
            new PropertyMetadata("No results found.", new PropertyChangedCallback(OnNoResultsMessagePropertyChanged)));

        private static void OnNoResultsMessagePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SearchTextBox searchTextBox = (SearchTextBox)sender;
            if (!string.IsNullOrEmpty((string)args.NewValue) && searchTextBox.NoResultsTemplate != null)
                throw new Exception("Cannot set both NoResultsMessage and NoResultsTemplate.");
        }

        public string NoResultsMessage
        {
            get { return (string)GetValue(NoResultsMessageProperty); }
            set { SetValue(NoResultsMessageProperty, value); }
        }

        #endregion

        #region NoResultsTemplate

        public static readonly DependencyProperty NoResultsTemplateProperty = DependencyProperty.Register("NoResultsTemplate",
            typeof(DataTemplate),
            typeof(SearchTextBox),
            new PropertyMetadata(null, new PropertyChangedCallback(OnNoResultsTemplatePropertyChanged)));

        private static void OnNoResultsTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SearchTextBox searchTextBox = (SearchTextBox)sender;
            if (!string.IsNullOrEmpty(searchTextBox.NoResultsMessage) && args.NewValue != null)
                throw new Exception("Cannot set both NoResultsMessage and NoResultsTemplate.");
        }

        public DataTemplate NoResultsTemplate
        {
            get { return (DataTemplate)GetValue(NoResultsTemplateProperty); }
            set { SetValue(NoResultsTemplateProperty, value); }
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current search worker
        /// </summary>
        BackgroundWorker CurrentWorker { get; set; }

        /// <summary>
        /// Gets the default template for display when searching
        /// </summary>
        DataTemplate DefaultSearchingTemplate
        {
            get
            {
                if (_defaultSearchingTemplate == null)
                    _defaultSearchingTemplate = Resources["DefaultSearchingTemplate"] as DataTemplate;
                return _defaultSearchingTemplate;
            }
        }
        DataTemplate _defaultSearchingTemplate;

        /// <summary>
        /// Gets the default template for displaying results
        /// </summary>
        DataTemplate DefaultResultsTemplate
        {
            get
            {
                if (_defaultResultsTemplate == null)
                    _defaultResultsTemplate = Resources["DefaultResultsTemplate"] as DataTemplate;
                return _defaultResultsTemplate;
            }
        }
        DataTemplate _defaultResultsTemplate;

        /// <summary>
        /// Gets the default data template for display when there are no results
        /// </summary>
        DataTemplate DefaultNoResultsTemplate
        {
            get
            {
                if (_defaultNoResultsTemplate == null)
                    _defaultNoResultsTemplate = Resources["DefaultNoResultsTemplate"] as DataTemplate;
                return _defaultNoResultsTemplate;
            }
        }
        DataTemplate _defaultNoResultsTemplate;

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
                Control resultsControl = ResultsPanel.Content as Control;
                if (resultsControl != null)
                {
                    // focus the results control
                    resultsControl.Focus();

                    // if the control is a selector, set the selected index to the first item
                    if (resultsControl is Selector)
                        ((Selector)resultsControl).SelectedIndex = 0;
                }
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
                if (SearchingTemplate != null)
                    ResultsPanel.Content = SearchingTemplate.LoadContent();
                else
                {
                    ResultsPanel.Content = DefaultSearchingTemplate.LoadContent();
                    ((TextBlock)((StackPanel)ResultsPanel.Content).Children[0]).Text = SearchingMessage;
                }
                
                // set the width
                ResultsPanel.Width = InputTextBox.ActualWidth;
                if (ResultsPanel.Content is FrameworkElement)
                    ((FrameworkElement)ResultsPanel.Content).Width = InputTextBox.ActualWidth;

                // if there are any bound items, filter them
                if (ItemsSource != null && ItemsSource.Cast<object>().Any())
                {
                    if (CurrentWorker != null)
                        CurrentWorker.CancelAsync();

                    CurrentWorker = GetSearchWorker();
                    CurrentWorker.RunWorkerAsync(new object[]
                    {
                        ItemsSource.Cast<object>(),
                        SearchMemberPath,
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
            string searchMemberPath = (string)((object[])e.Argument)[1];
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
                        if (!string.IsNullOrEmpty(searchMemberPath))
                        {
                            PropertyInfo displayProperty = o.GetType().GetProperty(searchMemberPath);
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
                if (results.Cast<object>().Any())
                {
                    // check if the results template is set; if not, use the default
                    if (ResultsTemplate != null)
                        ResultsPanel.Content = ResultsTemplate.LoadContent();
                    else
                    {
                        ResultsPanel.Content = DefaultResultsTemplate.LoadContent();
                        ((ListBox)ResultsPanel.Content).ItemContainerStyle = ItemContainerStyle;
                        ((ListBox)ResultsPanel.Content).ItemsPanel = ItemsPanel;
                        ((ListBox)ResultsPanel.Content).ItemTemplate = ItemTemplate;
                    }

                    // if the content supports items binding, bind the collection
                    if (ResultsPanel.Content is ItemsControl)
                        ((ItemsControl)ResultsPanel.Content).ItemsSource = results;
                }
                else
                {
                    // check if there's a template for no results; if not, use the default and set the message
                    if (NoResultsTemplate != null)
                        ResultsPanel.Content = NoResultsTemplate.LoadContent();
                    else
                    {
                        ResultsPanel.Content = DefaultNoResultsTemplate.LoadContent();
                        ((TextBlock)((StackPanel)ResultsPanel.Content).Children.First()).Text = NoResultsMessage;
                    }
                }

                if (ResultsPanel.Content is FrameworkElement)
                    ((FrameworkElement)ResultsPanel.Content).Width = InputTextBox.ActualWidth;
            }
        }

        #endregion
    }
}
