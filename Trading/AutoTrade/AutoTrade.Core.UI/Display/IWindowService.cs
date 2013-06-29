namespace AutoTrade.Core.UI.Display
{
    public interface IWindowService
    {
        /// <summary>
        /// Shows the view model in a non-modal window
        /// </summary>
        /// <param name="viewModel"></param>
        void ShowInWindow(ViewModelBase viewModel);

        /// <summary>
        /// Shows the view model in a modal window
        /// </summary>
        /// <param name="viewModel"></param>
        bool? ShowInModalWindow(ViewModelBase viewModel);
    }
}
