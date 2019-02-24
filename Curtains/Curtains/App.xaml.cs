using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Curtains.Views;
using Curtains.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Curtains
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SettingsPage());
        }

        protected override void OnResume()
        {
            if (BaseViewModel.DataConnection?.Client?.IsConnected == false)
            {
                MainPage = new NavigationPage(new SettingsPage());
            }
        }
    }
}
