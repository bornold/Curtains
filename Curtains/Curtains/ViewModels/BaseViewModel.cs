using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Curtains.Models;
using Curtains.Services;

namespace Curtains.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        internal static IConnection<CronJob> DataConnection;
        public IConnection<CronJob> Connection => DataConnection;
        public string Command { get; set; } = "python ~/motor/open.py";
        public bool IsConnected => Connection?.Client?.IsConnected ?? false;


        bool isBusy = false;
        public virtual bool IsBusy
        {
            get => isBusy; 
            set => SetProperty(ref isBusy, value);
        }

        string title = string.Empty;
        public string Title
        {
            get => title; 
            set => SetProperty(ref title, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
