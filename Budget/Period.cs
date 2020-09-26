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

        private DateTime Start { get; set; }
        private DateTime End   { get; set; }

        public int OverlappingDays(Budget budget)
        {
            var overlappingEnd = End;
            var overlappingStart = Start;
            if (Start.ToString("yyyyMM") != End.ToString("yyyyMM"))
            {
                if (budget.YearMonth == Start.ToString("yyyyMM"))
                {
                    overlappingEnd = budget.LastDay();
                    overlappingStart = Start;
                }
                else if (budget.YearMonth == End.ToString("yyyyMM"))
                {
                    overlappingEnd = End;
                    overlappingStart = budget.FirstDay();
                }
                else
                {
                    overlappingEnd = budget.LastDay();
                    overlappingStart = budget.FirstDay();
                }
            }

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}