using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TaskManagerApp.ViewModels
{
    /// <summary>
    /// Abstract Base ViewModel that provides standard INotifyPropertyChanged plumbing
    /// and shared properties for page states and navigation.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private bool _isBusy;

        /// <summary>
        /// Page or section Title.
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Indicates if a background process or operation is actively executing.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        /// <summary>
        /// Helper to access standard page navigation context.
        /// </summary>
        protected INavigation Navigation => Application.Current?.MainPage?.Navigation;

        /// <summary>
        /// Generic property setter helper that automatically triggers PropertyChanged notifications.
        /// </summary>
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the property changed notification for the bound UI to refresh.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
