using System.Threading.Tasks;
using Xamarin.Essentials;
using Curtains.Services;
using System;
using System.Reflection;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Curtains.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        readonly string 
            passKeyKey      = nameof(passKeyKey),
            userNameKey     = nameof(userNameKey),
            hostKey         = nameof(hostKey),
            portKey         = nameof(portKey);


        public bool
            IsConnectable =>
                !IsConnected &&
                !IsBusy &&
                ValidHost &&
                ValidPort &&
                ValidUserName &&
                ValidPassKey;

        public string Host
        {
            get => Preferences.Get(hostKey, "192.168.1.227");
            set => Preferences.Set(hostKey, value);
        }
        public bool ValidHost => IPAddress.TryParse(Host, out var ip);

        public int Port
        {
            get => Preferences.Get(portKey, 22);
            set => Preferences.Set(portKey, value);
        }
        public bool ValidPort => Port > 0 && Port < 65535;

        public string UserName
        {
            get => Preferences.Get(userNameKey, "pi");
            set => Preferences.Set(userNameKey, value);
        }
        public bool ValidUserName => !string.IsNullOrWhiteSpace(UserName);

        public string PassKey { get; set; }
        public bool ValidPassKey => 
            !string.IsNullOrWhiteSpace(PassKey) ||
            !string.IsNullOrWhiteSpace(StoredPassKey);

        string storedPassKey;
        string StoredPassKey
        {
            get => storedPassKey;
            set
            {
                if (value != null)
                {
                    storedPassKey = value;
                    OnPropertyChanged(nameof(HasStoredKey));
                }
            }
        }
        public bool HasStoredKey => StoredPassKey != null;

        public bool AutoConnect
        {
            get => Preferences.Get(nameof(AutoConnect), false);
            set => Preferences.Set(nameof(AutoConnect), value);
        }

        string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }
        public SettingsViewModel() => Title = "Settings";

        internal async Task<bool> OnAppearing()
        {
            IsBusy = true;
            try
            {
                StoredPassKey = await SecureStorage.GetAsync(passKeyKey);

                if (AutoConnect &&
                    !string.IsNullOrEmpty(StoredPassKey) &&
                    !string.IsNullOrEmpty(UserName) &&
                    !string.IsNullOrEmpty(Host))
                {
                    return await Connect();
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal async Task<bool> Connect()
        {
            IsBusy = true;
            try
            {
                var privateKeyStream = 
                    Assembly
                        .GetExecutingAssembly()
                        .GetManifestResourceStream("Curtains.Key.curt");
                        
                var pass = string.IsNullOrEmpty(PassKey) ? StoredPassKey : PassKey;
                DataConnection = new SSHConnection(Host, Port, UserName, pass, privateKeyStream);
                OnPropertyChanged(nameof(IsConnected));

                if (!string.IsNullOrEmpty(PassKey)) await SecureStorage.SetAsync(passKeyKey, PassKey);

                ErrorMessage = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                ErrorMessage = e.Message;
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal void Disconnect()
        {
            Connection?.Client?.Dispose();
            DataConnection = null;
            OnPropertyChanged(nameof(IsConnected));
        }
    }
}