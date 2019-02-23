using System;
using Curtains.Models;
using Curtains.ViewModels;
using Xunit;

namespace Tests
{
    public class ParseCrontabTest
    {
        AlarmDetailViewModel advm = new AlarmDetailViewModel();

        [Fact]
        public void ParsingHours()
        {
            var (_, actual) = advm.ParseAlarm("0 7 0 0 0 test");

            Assert.Equal(TimeSpan.FromHours(7), actual);
        }

        [Fact]
        public void ParsingHoursOver12()
        {
            var (_, actual) = advm.ParseAlarm("0 23 0 0 0 test");

            Assert.Equal(TimeSpan.FromHours(23), actual);
        }

        [Fact]
        public void ParsingMin()
        {
            var (_, actual) = advm.ParseAlarm("7 0 0 0 0 test");

            Assert.Equal(TimeSpan.FromMinutes(7), actual);
        }

        [Fact]
        public void ParsingMinOver12()
        {
            var (_, actual) = advm.ParseAlarm("59 0 0 0 0 test");

            Assert.Equal(TimeSpan.FromMinutes(59), actual);
        }


        [Fact]
        public void ParsingMonDays()
        {
            var (actual, _) = advm.ParseAlarm("59 0 0 0 mon test");

            Assert.Equal(Days.mon, actual);
        }

        [Fact]
        public void ParsingMonTueDays()
        {
            var (actual, _) = advm.ParseAlarm("59 0 0 0 mon,tue test");

            Assert.Equal(Days.mon | Days.tue, actual);
        }

        [Fact]
        public void ParsingAllDays()
        {
            var (actual, _) = advm.ParseAlarm("59 0 0 0 sun,mon,tue,wed,thu,fri,sat test");

            Assert.Equal(Days.all, actual);
        }

        [Fact]
        public void ParsingAllNotSunSatDays()
        {
            var (actual, _) = advm.ParseAlarm("59 0 0 0 mon,tue,wed,thu,fri test");

            Assert.Equal(Days.mon | Days.tue | Days.wed | Days.thu | Days.fri, actual);
        }
        [Fact]
        public void ParsingStarDays()
        {
            var (actual, _) = advm.ParseAlarm("* * * * * test");

            Assert.Equal(Days.non, actual);
        }
    }
}
