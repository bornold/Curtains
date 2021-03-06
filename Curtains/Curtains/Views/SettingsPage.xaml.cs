﻿using Curtains.ViewModels;
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
                apperedOnce = true;
                if (await viewModel.OnAppearing())
                {
                    await ChangePage();
                }
            }
            else if (viewModel.IsConnected)
            {
                if (ToolbarItems.Count < 1)
                {
                    var disconnect = new ToolbarItem() { Text = "Disconnect" };
                    disconnect.Clicked += Disconnect_Clicked;
                    ToolbarItems.Add(disconnect);
                }
            }
            else ToolbarItems.Clear();
        }

        async void Button_Clicked(object sender, System.EventArgs e)
        {
            if (viewModel.IsConnected || await viewModel.Connect())
            {
                await ChangePage();
            }
        }

        void Disconnect_Clicked(object sender, System.EventArgs e)
        {
            viewModel.Disconnect();
            ToolbarItems.Clear();
        }

        Task ChangePage() => Navigation.PushAsync(new AlarmsPage());
    }
}