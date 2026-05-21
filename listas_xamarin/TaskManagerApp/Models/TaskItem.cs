using SQLite;

namespace TaskManagerApp.Models
{
    /// <summary>
    /// Represents a task item entity stored in the SQLite database.
    /// </summary>
    [Table("Tasks")]
    public class TaskItem
    {
        /// <summary>
        /// Unique identifier for the task, configured as primary key and auto-incrementing.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Name or title of the task. Must be validated so it is not empty.
        /// </summary>
        [NotNull]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Short description of the task details.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
