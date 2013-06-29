using System;
using System.Windows;

namespace AutoTrade.Core.UI.Display
{
    public class WindowService : IWindowService
    {
        /// <summary>
        /// Shows the view model in a non-modal window
        /// </summary>
        /// <param name="viewModel"></param>
        public void ShowInWindow(ViewModelBase viewModel)
        {
            var window = GetWindow(viewModel);

            if (window != null) window.Show();
        }

        /// <summary>
        /// Shows the view model in a modal window
        /// </summary>
        /// <param name="viewModel"></param>
        public bool? ShowInModalWindow(ViewModelBase viewModel)
        {
            var window = GetWindow(viewModel);

            return window != null ? window.ShowDialog() : null;
        }

        /// <summary>
        /// Gets a <see cref="Window"/> to display the associated content for a <see cref="ViewModelBase"/>
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static Window GetWindow(ViewModelBase viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            // get the view template
            var viewTemplate = Application.Current.TryFindResource(new DataTemplateKey(viewModel.GetType())) as DataTemplate;

            // can't do anything if no view type found for view model
            if (viewTemplate == null) return null;

            // get content from data template
            var view = viewTemplate.LoadContent();

            // if view is Window, just show it - otherwise, create window and wrap content in it
            return view as Window ?? new Window { SizeToContent = SizeToContent.WidthAndHeight, Content = view };
        }
    }
}