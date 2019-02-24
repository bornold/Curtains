using System;

namespace Curtains.Models
{
    public class CronJob
    {
        public CronJob() { }

        public CronJob(string command, TimeSpan time, Days days = Days.all)
        {
            var cdays = DaysToCronDays(days);
            Raw = $"{time.Minutes} {time.Hours} * * {cdays} {command}";
        }
        public string Raw { get; set; }
        string DaysToCronDays(Days days) => days == Days.all || days == Days.non ? "*" : string.Join(string.Empty, days.ToString().Split(' '));
    }
}
