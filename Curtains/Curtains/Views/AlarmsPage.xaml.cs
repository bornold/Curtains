using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Curtains.ViewModels;
using Curtains.Models;

namespace Curtains.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlarmsPage : ContentPage
    {
        AlarmsViewModel viewModel;

        public AlarmsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new AlarmsViewModel();
            viewModel.Connection.Client.ErrorOccurred += Client_ErrorOccurred;
        }

        void Client_ErrorOccurred(object sender, Renci.SshNet.Common.ExceptionEventArgs e) => Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem is CronJob item)
                await Navigation.PushAsync(new AlarmDetailPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new AlarmDetailPage());

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadItemsCommand.Execute(null);
        }
        void Open_Clicked(object sender, EventArgs e) => viewModel.RunCommand.Execute(null);
    }
}