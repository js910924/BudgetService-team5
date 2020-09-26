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
                if (end < budget.FirstDay()||start>budget.LastDay())
                {
                    continue; 
                }
                var overlappingDays = new Period(start, end).OverlappingDays(budget);
                totalBudget += budget.DailyAmount() * overlappingDays;
            }

            return totalBudget;
        }
    }
}