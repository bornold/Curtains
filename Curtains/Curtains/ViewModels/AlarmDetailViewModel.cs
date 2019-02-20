using Curtains.Models;
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
            Time = ParseTime(item.Raw);
        }

        Days Days;
        public TimeSpan Time { get; set; }

        public bool Monday      { get => Has(Days.mon); set => Set(Days.mon); }
        public bool Tuesday     { get => Has(Days.tue); set => Set(Days.tue); }
        public bool Wednesday   { get => Has(Days.wed); set => Set(Days.wed); }
        public bool Thursday    { get => Has(Days.thu); set => Set(Days.thu); }
        public bool Friday      { get => Has(Days.fri); set => Set(Days.fri); }
        public bool Saturday    { get => Has(Days.sat); set => Set(Days.sat); }
        public bool Sunday      { get => Has(Days.sun); set => Set(Days.sun); }
        bool Has(Days day) => Days.HasFlag(day);
        void Set(Days day) => Days |= day;

        public string Command { get; set; } = "python ~/motor/open.py";

        internal Task DeleteItem() => DataStore.DeleteItem(Item?.Raw);

        internal Task AddItem() =>
            NewItem ?
            DataStore.AddItem(new CronJob(Command, Time, Days)) :
            DataStore.UpdateItem(Item.Raw, new CronJob(Command, Time, Days));

        TimeSpan ParseTime(string raw)
        {
            string[] seperated = raw.Split(' ');
            if (seperated.Length < 5)
                return TimeSpan.Zero;
            if (TimeSpan.TryParse($"{seperated[1]}:{seperated[0]}", out var time))
                return time;
            return TimeSpan.Zero;
        }
    }
}