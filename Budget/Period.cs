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
            DateTime overlappingStart = Start;
            DateTime overlappingEnd = End;
            if (Start.ToString("yyyyMM") == End.ToString("yyyyMM"))
            {
                if (budget.YearMonth == Start.ToString("yyyyMM"))
                {
                    overlappingEnd = End;
                    overlappingStart = Start;
                }
            }
            else
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
                else if (budget.FirstDay() >= Start && budget.FirstDay() <= End)
                {
                    overlappingEnd = budget.LastDay();
                    overlappingStart = budget.FirstDay();
                }
            }

            var overlappingDays = ((overlappingEnd - overlappingStart).Days + 1);
            return overlappingDays;
        }
    }
}