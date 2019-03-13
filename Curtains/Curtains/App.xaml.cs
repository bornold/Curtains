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

        protected override async void OnResume()
        {
            if (BaseViewModel.DataConnection?.Client?.IsConnected == false && MainPage is NavigationPage navpage)
            {
                await navpage.PopToRootAsync();
            }
        }
    }
}
