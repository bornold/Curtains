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

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            BaseViewModel.DataConnection?.Dispose();
            BaseViewModel.DataConnection = null;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
