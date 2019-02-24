using System.Threading.Tasks;
using Xamarin.Essentials;
using Plugin.FilePicker.Abstractions;
using System.IO;
using Curtains.Helpers;
using Curtains.Services;
using Plugin.FilePicker;
using System;

namespace Curtains.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        readonly string 
            PassKeyKey      = nameof(PassKeyKey),
            PrivateKeyKey   = nameof(PrivateKeyKey),
            UserNameKey     = nameof(UserNameKey),
            HostKey         = nameof(HostKey),
            PortKey         = nameof(PortKey);

        public string Host
        {
            get => Preferences.Get(HostKey, null);
            set => Preferences.Set(HostKey, value);
        }
        public int Port
        {
            get => Preferences.Get(PortKey, 22);
            set => Preferences.Set(PortKey, value);
        }
        public string UserName
        {
            get => Preferences.Get(UserNameKey, null);
            set => Preferences.Set(UserNameKey, value);
        }
        public string PassKey { get; set; }

        string storedKey;
        string StoredPassKey
        {
            get => storedKey;
            set
            {
                if (value != null)
                {
                    storedKey = value;
                    OnPropertyChanged(nameof(HasStoredKey));
                }
            }
        }
        public bool HasStoredKey => storedKey != null;

        string PrivateKey { get; set; }
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
            try
            {
                IsBusy = true;

                StoredPassKey = await SecureStorage.GetAsync(PassKeyKey);
                PrivateKey = await SecureStorage.GetAsync(PrivateKeyKey);

                if (StoredPassKey != null && 
                    PrivateKey != null && 
                    UserName != null && 
                    Host != null &&
                    AutoConnect)
                {
                    return await Connect();
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal async Task<bool> Connect()
        {
            try
            {
                Stream privateKeyStream;
                if (PrivateKey != null)
                {
                    privateKeyStream = PrivateKey.GenerateStreamFromString();
                }
                else
                {
                    var fileData = await CrossFilePicker.Current.PickFile();
                    if (fileData == null)
                        return false;
                    PrivateKey = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                    privateKeyStream = fileData.GetStream();
                }

                var pass = string.IsNullOrEmpty(PassKey) ? StoredPassKey : PassKey;
                DataConnection = new SSHConnection(Host, Port, UserName, pass, privateKeyStream);
                OnPropertyChanged(nameof(IsConnected));
                await SecureStorage.SetAsync(PassKeyKey, pass);
                await SecureStorage.SetAsync(PrivateKeyKey, PrivateKey);
                ErrorMessage = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
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