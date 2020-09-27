using System;
using System.Collections.Generic;
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
            var period = new Period(start, end);

            return _repo
                .GetAll()
                .Sum(budget => budget.OverlappingAmount(period));
        }
    }
}