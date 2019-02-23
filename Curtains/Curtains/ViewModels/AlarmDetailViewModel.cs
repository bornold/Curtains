﻿using Curtains.Models;
using System;
using System.Threading.Tasks;

namespace Curtains.ViewModels
{
    public class AlarmDetailViewModel : BaseViewModel
    {
        bool NewItem => Item == null;
        public CronJob Item { get; set; }

        public AlarmDetailViewModel()
        {
            Title = "New alarm";
            Time = TimeSpan.FromHours(7);
        }

        public AlarmDetailViewModel(CronJob item)
        {
            Title = "Change alarm";
            Item = item;
            (days, Time) = ParseAlarm(item.Raw);
        }

        Days days;
        public TimeSpan Time { get; set; }

        public bool Monday      { get => Has(Days.mon); set => Set(Days.mon); }
        public bool Tuesday     { get => Has(Days.tue); set => Set(Days.tue); }
        public bool Wednesday   { get => Has(Days.wed); set => Set(Days.wed); }
        public bool Thursday    { get => Has(Days.thu); set => Set(Days.thu); }
        public bool Friday      { get => Has(Days.fri); set => Set(Days.fri); }
        public bool Saturday    { get => Has(Days.sat); set => Set(Days.sat); }
        public bool Sunday      { get => Has(Days.sun); set => Set(Days.sun); }
        bool Has(Days day) => days.HasFlag(day);
        void Set(Days day) => days |= day;

        public string Command { get; set; } = "python ~/motor/open.py";

        internal Task DeleteItem() => DataStore.DeleteItem(Item?.Raw);

        internal Task AddItem() =>
            NewItem ?
            DataStore.AddItem(new CronJob(Command, Time, days)) :
            DataStore.UpdateItem(Item.Raw, new CronJob(Command, Time, days));

        public (Days, TimeSpan) ParseAlarm(string raw)
        {
            var seperated = raw.Split(' ');
            if (seperated.Length < 5)
                return (Days.non, TimeSpan.Zero);
            TimeSpan.TryParse($"{seperated[1]}:{seperated[0]}", out var time);

            if (Enum.TryParse(seperated[4], true, out Days dayValue) &&
                (Enum.IsDefined(typeof(Days), dayValue) | dayValue.ToString().Contains(",")))
                return (dayValue, time);
            
            return (Days.non, time);
        }
    }
}