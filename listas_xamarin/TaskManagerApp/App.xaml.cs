using Xamarin.Forms;
using TaskManagerApp.Views;

namespace TaskManagerApp
{
    /// <summary>
    /// Global Application class, acts as the root entry point of Xamarin.Forms execution.
    /// </summary>
    public partial class App : Application
    {
        private static Services.DatabaseService _database;
        public static Services.DatabaseService Database
        {
            get
            {
                if (_database == null)
                {
                    var dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "TaskManager.db3");
                    _database = new Services.DatabaseService(dbPath);
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();

            // Wraps our MainPage in a styled NavigationPage to support page pushing and popping
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Fired when the app launches
        }

        protected override void OnSleep()
        {
            // Fired when the app goes to the background
        }

        protected override void OnResume()
        {
            // Fired when the app is resumed from background
        }
    }
}
