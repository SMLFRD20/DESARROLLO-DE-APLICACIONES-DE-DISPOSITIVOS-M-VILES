using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManagerApp.Models;
using TaskManagerApp.Services;
using TaskManagerApp.Views;
using Xamarin.Forms;

namespace TaskManagerApp.ViewModels
{
    /// <summary>
    /// ViewModel for the Main dashboard list screen, managing load commands, creation triggers, and detailed task navigation.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private TaskItem _selectedTask;

        /// <summary>
        /// Bindable collection of task list items.
        /// </summary>
        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();

        /// <summary>
        /// Command to asynchronously retrieve records from SQLite.
        /// </summary>
        public ICommand LoadTasksCommand { get; }

        /// <summary>
        /// Command to initiate creation of a new task.
        /// </summary>
        public ICommand AddTaskCommand { get; }

        /// <summary>
        /// Command to navigate to details/edit of a specific item.
        /// </summary>
        public ICommand SelectTaskCommand { get; }

        /// <summary>
        /// Holds the currently selected task item. Automatically initiates detail navigation on value changes.
        /// </summary>
        public TaskItem SelectedTask
        {
            get => _selectedTask;
            set
            {
                SetProperty(ref _selectedTask, value);
                OnTaskSelected(value);
            }
        }

        public MainViewModel()
        {
            Title = "Gestor de Tareas";
            LoadTasksCommand = new Command(async () => await ExecuteLoadTasksCommand());
            AddTaskCommand = new Command(async () => await NavigateToAddTask());
            SelectTaskCommand = new Command<TaskItem>(async (task) => await NavigateToEditTask(task));
        }

        /// <summary>
        /// Safe database fetch that resets the active collection and handles unexpected database exceptions gracefully.
        /// </summary>
        public async Task ExecuteLoadTasksCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Tasks.Clear();
                var tasks = await App.Database.GetTasksAsync();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading tasks: {ex}");
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error", 
                        $"No se pudo cargar la lista de tareas: {ex.Message}", 
                        "Aceptar");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToAddTask()
        {
            try
            {
                // Navigate to EditTaskPage passing a blank new task model
                if (Navigation != null)
                {
                    await Navigation.PushAsync(new EditTaskPage(new TaskItem()));
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error de Navegación", 
                        $"No se pudo abrir la pantalla de creación de tareas: {ex.Message}", 
                        "Aceptar");
                }
            }
        }

        private async Task NavigateToEditTask(TaskItem task)
        {
            if (task == null)
                return;

            try
            {
                // Navigate to EditTaskPage passing the existing selected task
                if (Navigation != null)
                {
                    await Navigation.PushAsync(new EditTaskPage(task));
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error de Navegación", 
                        $"No se pudo abrir la pantalla de edición de tareas: {ex.Message}", 
                        "Aceptar");
                }
            }
        }

        private async void OnTaskSelected(TaskItem task)
        {
            if (task == null)
                return;

            await NavigateToEditTask(task);
            
            // Clear the selected value so the UI is ready to select it again
            SelectedTask = null;
        }
    }
}
