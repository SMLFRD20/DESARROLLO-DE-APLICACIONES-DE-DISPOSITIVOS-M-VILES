using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManagerApp.Models;
using TaskManagerApp.Services;
using Xamarin.Forms;

namespace TaskManagerApp.ViewModels
{
    /// <summary>
    /// ViewModel for the Add/Edit Task screen. Isolates edits to local variables and provides validations/confirmations.
    /// </summary>
    public class EditTaskViewModel : BaseViewModel
    {
        private readonly TaskItem _originalTask;
        private int _id;
        private string _name = string.Empty;
        private string _description = string.Empty;

        /// <summary>
        /// Task entity Identifier (0 indicates a new item).
        /// </summary>
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// Input property for the Task's name/title.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Input property for the Task's details/description.
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        /// <summary>
        /// Tells if the task is an existing record, used to toggle Delete button visibility.
        /// </summary>
        public bool IsExistingTask => Id != 0;

        /// <summary>
        /// Command to validate and save changes to SQLite database.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Command to request and execute task deletion.
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Command to discard changes and pop back to the dashboard.
        /// </summary>
        public ICommand CancelCommand { get; }

        public EditTaskViewModel(TaskItem task)
        {
            _originalTask = task ?? new TaskItem();
            
            // Map original task properties to local edit state
            Id = _originalTask.Id;
            Name = _originalTask.Name;
            Description = _originalTask.Description;

            // Set screen title dynamically based on insert or update mode
            Title = IsExistingTask ? "Editar Tarea" : "Nueva Tarea";

            // Initialize command callbacks
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            DeleteCommand = new Command(async () => await ExecuteDeleteCommand());
            CancelCommand = new Command(async () => await ExecuteCancelCommand());
        }

        /// <summary>
        /// Validates input data, commits updates to the SQLite entity, saves, and pops navigation.
        /// </summary>
        private async Task ExecuteSaveCommand()
        {
            // Requirement 6: Validate empty task names before saving
            if (string.IsNullOrWhiteSpace(Name))
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error de Validación", 
                        "El nombre de la tarea es obligatorio y no puede estar vacío.", 
                        "Aceptar");
                }
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                // Assign validated text inputs to core model
                _originalTask.Name = Name.Trim();
                _originalTask.Description = Description?.Trim() ?? string.Empty;

                int rows = await App.Database.SaveTaskAsync(_originalTask);
                bool success = rows > 0;
                if (success)
                {
                    // Success! Pop page back and return to dashboard
                    if (Navigation != null)
                    {
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Error al Guardar", 
                            "No se pudo guardar la tarea en la base de datos.", 
                            "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error", 
                        $"Ocurrió un error inesperado: {ex.Message}", 
                        "Aceptar");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Prompt for delete confirmation, deletes the record from database, and pops navigation.
        /// </summary>
        private async Task ExecuteDeleteCommand()
        {
            if (!IsExistingTask)
                return;

            if (Application.Current?.MainPage == null)
                return;

            // Get explicit user confirmation
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Eliminar Tarea", 
                "¿Estás seguro de que deseas eliminar esta tarea para siempre?", 
                "Sí", "No");

            if (!confirm)
                return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                int rows = await App.Database.DeleteTaskAsync(_originalTask);
                bool success = rows > 0;
                if (success)
                {
                    if (Navigation != null)
                    {
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error al Eliminar", 
                        "No se pudo eliminar la tarea de la base de datos.", 
                        "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error", 
                    $"Ocurrió un error inesperado al eliminar: {ex.Message}", 
                    "Aceptar");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Discards edits and returns to the task list page safely.
        /// </summary>
        private async Task ExecuteCancelCommand()
        {
            try
            {
                if (Navigation != null)
                {
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error", 
                        $"No se pudo regresar a la lista: {ex.Message}", 
                        "Aceptar");
                }
            }
        }
    }
}
