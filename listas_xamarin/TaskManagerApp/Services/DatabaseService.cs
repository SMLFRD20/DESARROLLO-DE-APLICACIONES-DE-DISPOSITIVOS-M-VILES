using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using TaskManagerApp.Models;

namespace TaskManagerApp.Services
{
    /// <summary>
    /// Servicio de base de datos simplificado para el almacenamiento local con SQLite.
    /// Diseñado para ser fácil de entender en un entorno académico básico.
    /// </summary>
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _databaseConnection;

        /// <summary>
        /// Constructor que inicializa la conexión y crea la tabla de tareas de forma automática.
        /// </summary>
        /// <param name="dbPath">Ruta completa al archivo de base de datos SQLite.</param>
        public DatabaseService(string dbPath)
        {
            _databaseConnection = new SQLiteAsyncConnection(dbPath);
            // Crea la tabla si no existe de forma síncrona en el constructor para simplificar el flujo
            _databaseConnection.CreateTableAsync<TaskItem>().Wait();
        }

        /// <summary>
        /// Obtiene todas las tareas guardadas.
        /// </summary>
        public Task<List<TaskItem>> GetTasksAsync()
        {
            return _databaseConnection.Table<TaskItem>().ToListAsync();
        }

        /// <summary>
        /// Obtiene una tarea específica por su ID.
        /// </summary>
        public Task<TaskItem> GetTaskAsync(int id)
        {
            return _databaseConnection.Table<TaskItem>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Guarda una tarea. Si ya existe la actualiza, si es nueva la inserta.
        /// </summary>
        public Task<int> SaveTaskAsync(TaskItem item)
        {
            if (item.Id != 0)
            {
                return _databaseConnection.UpdateAsync(item);
            }
            else
            {
                return _databaseConnection.InsertAsync(item);
            }
        }

        /// <summary>
        /// Elimina una tarea de la base de datos.
        /// </summary>
        public Task<int> DeleteTaskAsync(TaskItem item)
        {
            return _databaseConnection.DeleteAsync(item);
        }
    }
}
