namespace AutoTrade.Core.UI
{
    public interface IViewService
    {
        /// <summary>
        /// Gets the view for a view model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        object GetView<T>()  where T : ViewModelBase;

        /// <summary>
        /// Gets the view for a view model
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        object GetView(ViewModelBase viewModel);

        /// <summary>
        /// Displays the view for a view model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Display<T>() where T : ViewModelBase;

        /// <summary>
        /// Displays the view for a view model
        /// </summary>
        /// <param name="viewModelBase"></param>
        void Display(ViewModelBase viewModelBase);
    }
}
