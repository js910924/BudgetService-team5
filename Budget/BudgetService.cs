using System;
using System.Linq;
using NSubstitute;

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
        public DateTime End   { get; private set; }
    }

    public class BudgetService
    {
        private readonly IBudgetRepo _repo;

        public BudgetService(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public double Query(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return 0;
            }

            var budgets = _repo.GetAll();
            if (!budgets.Any())
            {
                return 0;
            }

            var totalBudget = 0;
            foreach (var budget in budgets)
            {
                if (end<budget.FirstDay()||start>budget.LastDay())
                {
                    continue;
                }
                var overlappingDays = OverlappingDays(new Period(start, end), budget);
                totalBudget += budget.DailyAmount() * overlappingDays;
            }

            return totalBudget;
        }

        private static int OverlappingDays(Period period, Budget budget)
        {
            var overlappingEnd = period.End;
            var overlappingStart = period.Start;
            if (period.Start.ToString("yyyyMM") == period.End.ToString("yyyyMM"))
            {
                if (budget.YearMonth == period.Start.ToString("yyyyMM"))
                {
                    overlappingEnd = period.End;
                    overlappingStart = period.Start;
                }
            }
            else
            {
                if (budget.YearMonth == period.Start.ToString("yyyyMM"))
                {
                    overlappingEnd = budget.LastDay();
                    overlappingStart = period.Start;
                }
                else if (budget.YearMonth == period.End.ToString("yyyyMM"))
                {
                    overlappingEnd = period.End;
                    overlappingStart = budget.FirstDay();
                }
                else if (budget.FirstDay() >= period.Start && budget.FirstDay() <= period.End)
                {
                    overlappingEnd = budget.LastDay();
                    overlappingStart = budget.FirstDay();
                }
            }

            var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            return overlappingDays;
        }
    }
}