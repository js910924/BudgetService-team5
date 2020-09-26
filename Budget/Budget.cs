#region

using System;
using System.Globalization;

#endregion

namespace Budget
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int    Amount    { get; set; }

        public int GetOverlappingAmount(Period period)
        {
            return DailyAmount() * period.OverlappingDays(CreatePeriod());
        }

        private Period CreatePeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        private int DailyAmount()
        {
            return Amount / Days();
        }

        private int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact($"{YearMonth}", "yyyyMM", CultureInfo.InvariantCulture);
        }

        private DateTime LastDay()
        {
            return new DateTime(FirstDay().Year, FirstDay().Month, Days());
        }
    }
}