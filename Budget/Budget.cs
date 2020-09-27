
using System;
using System.Globalization;

namespace Budget
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact($"{YearMonth}", "yyyyMM", CultureInfo.InvariantCulture);
        }

        private int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        private DateTime LastDay()
        {
            return new DateTime(FirstDay().Year, FirstDay().Month, Days());
        }

        private int DailyAmount()
        {
            return Amount / Days();
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