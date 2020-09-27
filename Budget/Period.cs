using System;

namespace Budget
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public int OverlappingDays(Budget budget)
        {
            var firstDay = budget.FirstDay();
            var lastDay = budget.LastDay();

            if (lastDay < Start || firstDay > End)
            {
                return 0;
            }

            var overlappingStart = Start > firstDay ? Start : firstDay;
            var overlappingEnd = End < lastDay ? End : lastDay;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}