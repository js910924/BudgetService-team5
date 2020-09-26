using System;
using System.Linq;
using NSubstitute;

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
                var overlappingEnd = end;
                var overlappingStart = start;
                if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
                {
                    totalBudget += budget.DailyAmount() * ((overlappingEnd - overlappingStart).Days + 1);
                }
                else
                {
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        totalBudget += budget.DailyAmount() * ((budget.LastDay() - start).Days + 1);
                    }
                    else if (budget.YearMonth == end.ToString("yyyyMM"))
                    {
                        totalBudget += budget.DailyAmount() * ((end - budget.FirstDay()).Days + 1);
                    }
                    else if (budget.FirstDay() >= start && budget.FirstDay() <= end)
                    {
                        totalBudget += budget.DailyAmount() * ((budget.LastDay() - budget.FirstDay()).Days + 1);
                    }
                }
            }

            return totalBudget;
        }
    }
}