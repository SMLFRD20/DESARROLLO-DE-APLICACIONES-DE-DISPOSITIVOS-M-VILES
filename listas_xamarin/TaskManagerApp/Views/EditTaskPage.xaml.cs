using Xamarin.Forms;
using TaskManagerApp.Models;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Views
{
    /// <summary>
    /// Code-behind class for the EditTaskPage.
    /// </summary>
    public partial class EditTaskPage : ContentPage
    {
        /// <summary>
        /// Constructor that receives a TaskItem and sets up its specialized Edit ViewModel.
        /// </summary>
        /// <param name="task">The task item model being created or modified.</param>
        public EditTaskPage(TaskItem task)
        {
            InitializeComponent();
            
            // Set up MVVM architecture by passing the injected TaskItem into the ViewModel
            BindingContext = new EditTaskViewModel(task);
        }
    }
}
