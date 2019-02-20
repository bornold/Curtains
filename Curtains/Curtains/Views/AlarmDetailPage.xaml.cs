using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Curtains.ViewModels;
using Curtains.Models;

namespace Curtains.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlarmDetailPage : ContentPage
    {
        AlarmDetailViewModel viewModel;

        public AlarmDetailPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AlarmDetailViewModel();
        }
        public AlarmDetailPage(CronJob item)
        {
            InitializeComponent();
            var deleteItem = new ToolbarItem()
            {
                Text = "Delete"
            };
            deleteItem.Clicked += Delete_Clicked;
            ToolbarItems.Add(deleteItem);
            BindingContext = viewModel = new AlarmDetailViewModel(item);
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Item != null)
                await viewModel.DeleteItem();
            await Navigation.PopAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            await viewModel.AddItem();
            await Navigation.PopAsync();
        }
    }
}