using System;
using System.Linq;

namespace Budget
{
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
                var overlappingDays = OverlappingDays(start, end, budget);
                totalBudget += budget.DailyAmount() * overlappingDays;
            }

            return totalBudget;
        }

        private static int OverlappingDays(DateTime start, DateTime end, Budget budget)
        {
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

            var overlappingDays = ((overlappingEnd - overlappingStart).Days + 1);
            return overlappingDays;
        }
    }
}