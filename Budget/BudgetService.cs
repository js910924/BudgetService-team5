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
            var period = new Period(start, end);
            foreach (var budget in budgets)
            { 
                totalBudget += OverlappingAmount(budget, period);
            }

            return totalBudget;
        }

        private static int OverlappingAmount(Budget budget, Period period)
        {
            return budget.DailyAmount() * period.OverlappingDays( budget.CreatePeriod());
        }
    }
}