
using System;
using System.Globalization;

namespace Budget
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int    Amount    { get; set; }

        private DateTime FirstDay()
        {
            var dateTime = DateTime.ParseExact($"{YearMonth}", "yyyyMM", CultureInfo.InvariantCulture);
            return dateTime;
        }

        private int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        private DateTime LastDay()
        {
            var lastOfMonth = new DateTime(FirstDay().Year, FirstDay().Month, Days());
            return lastOfMonth;
        }

        private int DailyAmount()
        {
            var dailyAmount = Amount / Days();
            return dailyAmount;
        }

        private Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        public int OverlappingAmount(Period period)
        {
            return DailyAmount() * period.OverlappingDays(CreatePeriod());
        }
    }
}