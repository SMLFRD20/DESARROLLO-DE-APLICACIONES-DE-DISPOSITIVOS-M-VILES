using Xamarin.Forms;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Views
{
    /// <summary>
    /// Code-behind class for the MainPage.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            
            // Instantiate the MainViewModel and assign it as the BindingContext for MVVM data binding
            BindingContext = _viewModel = new MainViewModel();
        }

        /// <summary>
        /// Page lifecycle method that executes when the screen comes into active focus.
        /// Reloads tasks from database to reflect any inserts, updates, or deletes.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Reload the task items automatically when navigating back to this page
            if (_viewModel != null)
            {
                await _viewModel.ExecuteLoadTasksCommand();
            }
        }
    }
}
