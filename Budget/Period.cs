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

        public int OverlappingDays(Period another)
        {
            if (IsInvalid() || WithoutOverlapping(another))
            {
                return 0;
            }

            var overlappingStart = Start > another.Start ? Start : another.Start;
            var overlappingEnd = End < another.End ? End : another.End;

            return (overlappingEnd - overlappingStart).Days + 1;
        }

        private bool WithoutOverlapping(Period another)
        {
            return another.End < Start || another.Start > End;
        }

        private bool IsInvalid()
        {
            return Start > End;
        }
    }
}