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
            var overlappingStart = Start > budget.FirstDay() ? Start : budget.FirstDay();
            var overlappingEnd = End < budget.LastDay() ? End : budget.LastDay();

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}