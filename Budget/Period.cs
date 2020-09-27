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
            var overlappingEnd = End<budget.LastDay()
                ?End
                :budget.LastDay();
            var overlappingStart = Start > budget.FirstDay()
                ?Start
                :budget.FirstDay(); 

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}