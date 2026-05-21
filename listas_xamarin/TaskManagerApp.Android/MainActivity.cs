using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace TaskManagerApp.Droid
{
    /// <summary>
    /// Core entry activity for the Android platform, launching Xamarin.Forms.
    /// </summary>
    [Activity(Label = "Gestor de Tareas", 
              Icon = "@drawable/icon", 
              Theme = "@style/MainTheme", 
              MainLauncher = true, 
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// Native Android entry point. Sets up Forms/Essentials platforms and mounts the App.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Initialize platform APIs
            global::Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            // Launch the shared Xamarin.Forms application
            LoadApplication(new App());
        }

        /// <summary>
        /// Routes native hardware and feature permissions back to Xamarin.Essentials.
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [global::Android.Runtime.GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            global::Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
