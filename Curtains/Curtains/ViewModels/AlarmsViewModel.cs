﻿using Curtains.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Curtains.ViewModels
{
    public class AlarmsViewModel : BaseViewModel
    {
        public ObservableCollection<CronJob> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public AlarmsViewModel()
        {
            Title = "Alarms";
            Items = new ObservableCollection<CronJob>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItems(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}