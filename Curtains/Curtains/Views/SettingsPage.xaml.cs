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
            if (!apperedOnce)
            {
                await Navigation.PushModalAsync(new Overlay(), true);
                if (await viewModel.OnAppearing())
                {
                    await ChangePage();
                }
                await Navigation.PopModalAsync(true);
            }
            else if (viewModel.IsConnected)
            {
                var disconnect = new ToolbarItem() { Text = "Disconnect" };
                disconnect.Clicked += Disconnect_Clicked;
                ToolbarItems.Add(disconnect);
            }

            apperedOnce = true;
        }

        async void Button_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new Overlay(), true);
            if (viewModel.IsConnected || await viewModel.Connect())
            {
                await ChangePage();
            }
            await Navigation.PopModalAsync(true);
        }

        void Disconnect_Clicked(object sender, System.EventArgs e)
        {
            viewModel.Disconnect();
            ToolbarItems.Clear();
        }

        Task ChangePage() => Navigation.PushAsync(new AlarmsPage());
    }
}