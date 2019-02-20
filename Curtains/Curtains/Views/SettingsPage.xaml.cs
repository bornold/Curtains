using Curtains.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Curtains.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingsViewModel();
        }

        bool apperedOnce = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!apperedOnce && await viewModel.OnAppearing())
            {
                await ChangePage();
            }
            apperedOnce = true;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            if (await viewModel.Connect())
            {
                await ChangePage();
            }
        }
        async Task ChangePage()
        {
            await Navigation.PushAsync(new AlarmsPage());
            Navigation.RemovePage(this);
        }
    }
}