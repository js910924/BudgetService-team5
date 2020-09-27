
using System;
using System.Globalization;

namespace Budget
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int    Amount    { get; set; }
        
        public DateTime FirstDay()
        {
            var dateTime = DateTime.ParseExact($"{YearMonth}", "yyyyMM", CultureInfo.InvariantCulture);
            return dateTime;
        }

        public int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        public DateTime LastDay()
        {
            var lastOfMonth = new DateTime(FirstDay().Year, FirstDay().Month, Days());
            return lastOfMonth;
        }

        public int DailyAmount()
        {
            var dailyAmount = Amount / Days();
            return dailyAmount;
        }
    }
}