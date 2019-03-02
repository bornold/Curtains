using System;
using System.Threading.Tasks;
using Curtains.Models;

namespace Curtains.ViewModels
{
    public class EditAlarmViewModel : AlarmDetailViewModel
    {
        public EditAlarmViewModel()
        {
            Title = "New alarm";
            Time = TimeSpan.FromHours(7);
        }
        public EditAlarmViewModel(CronJob item) : base(item) { }

        bool NewItem => Item == null;

        public string Command { get; set; } = "python ~/motor/open.py";

        internal Task DeleteItem() => DataConnection.DeleteItem(Item?.Raw);

        internal Task AddItem() =>
            NewItem ?
            DataConnection.AddItem(new CronJob(Command, Time, days)) :
            DataConnection.UpdateItem(Item.Raw, new CronJob(Command, Time, days));
    }
}
