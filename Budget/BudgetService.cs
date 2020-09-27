using System;
using System.Linq;

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
                if (budget.LastDay() < start || budget.FirstDay()>end)
                {
                    continue;
                }
                DateTime overlappingStart = start;
                DateTime overlappingEnd = end;
                if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingEnd = end;
                        overlappingStart = start;
                    }
                }
                else
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingEnd = budget.LastDay();
                        overlappingStart = start;
                    }
                    else if (budget.YearMonth == end.ToString("yyyyMM"))
                    {
                        overlappingEnd = end;
                        overlappingStart = budget.FirstDay();
                    }
                    else if (budget.FirstDay() >= start && budget.FirstDay() <= end)
                    {
                        overlappingEnd = budget.LastDay();
                        overlappingStart = budget.FirstDay();
                    }
                }

                var overlappingDays = OverlappingDays(new Period(overlappingStart, overlappingEnd));
                totalBudget += budget.DailyAmount() * overlappingDays;
            }

            return totalBudget;
        }

        private static int OverlappingDays(Period period)
        {
            var overlappingDays = ((period.End - period.Start).Days + 1);
            return overlappingDays;
        }
    }
}