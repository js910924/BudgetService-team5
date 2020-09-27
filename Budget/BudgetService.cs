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
            var period = new Period(start, end);
            foreach (var budget in budgets)
            {
                totalBudget += budget.DailyAmount() * period.OverlappingDays(budget.CreatePeriod());
            }

            return totalBudget;
        }
    }
}