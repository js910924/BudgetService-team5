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
                DateTime overlappingStart;
                DateTime overlappingEnd;
                if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingEnd = end;
                        overlappingStart = start;
                        totalBudget += budget.DailyAmount() * ((overlappingEnd - overlappingStart).Days + 1);
                    }
                }
                else
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingEnd = budget.LastDay();
                        overlappingStart = start;
                        totalBudget += budget.DailyAmount() * ((overlappingEnd - overlappingStart).Days + 1);
                    }
                    else if (budget.YearMonth == end.ToString("yyyyMM"))
                    {
                        overlappingEnd = end;
                        overlappingStart = budget.FirstDay();
                        totalBudget += budget.DailyAmount() * ((overlappingEnd - overlappingStart).Days + 1);
                    }
                    else if (budget.FirstDay() >= start && budget.FirstDay() <= end)
                    {
                        overlappingEnd = budget.LastDay();
                        overlappingStart = budget.FirstDay();
                        totalBudget += budget.DailyAmount() * ((overlappingEnd - overlappingStart).Days + 1);
                    }
                }
            }

            return totalBudget;
        }
    }
}