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

        public int OverlappingDays(Budget budget)
        {
            var overlappingEnd = End;
            var overlappingStart = Start;
            if (Start.ToString("yyyyMM") != End.ToString("yyyyMM"))
            {
                if (budget.YearMonth == Start.ToString("yyyyMM"))
                {
                    overlappingEnd = budget.LastDay();
                    overlappingStart = Start;
                }
                else if (budget.YearMonth == End.ToString("yyyyMM"))
                {
                    overlappingEnd = End;
                    overlappingStart = budget.FirstDay();
                }
                else
                {
                    overlappingEnd = budget.LastDay();
                    overlappingStart = budget.FirstDay();
                }
            }

            var overlappingDays = ((overlappingEnd - overlappingStart).Days + 1);
            return overlappingDays;
        }
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