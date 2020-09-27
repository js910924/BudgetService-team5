using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Budget
{
    public class BudgetServiceTests
    {
        private BudgetService _budgetService;
        private DateTime _startDate;
        private DateTime _endDate;
        private IBudgetRepo _repo;
        private List<Budget> _budgets;

        [SetUp]
        public void Setup()
        {
            _repo = Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(_repo);
        }

        [Test]
        public void WhenEndDateGreaterThanStartDate()
        {
            _startDate = new DateTime(2020, 1, 1);
            _endDate = new DateTime(2019, 1, 1);
            AmountShouldBe(0);
        }

        [Test]
        public void WhenNoBudget()
        {
            GivenListOfBudgets(new List<Budget>());
            _repo.GetAll().Returns(_budgets);
            _startDate = new DateTime(2019, 08, 1);
            _endDate = new DateTime(2019, 09, 1);
            AmountShouldBe(0);
        }

        private void GivenListOfBudgets(List<Budget> budgets)
        {
            _budgets = budgets;
        }

        [Test]
        public void WhenQueryOneDay()
        {
            GivenListOfBudgets(new List<Budget>()
                               {
                                   new Budget { YearMonth = "202001" , Amount    = 310 },
                                   new Budget { YearMonth = "202002" , Amount    = 310 },
                                   new Budget { YearMonth = "202003" , Amount    = 310 },
                               });
            _repo.GetAll().Returns(_budgets);
            _startDate = new DateTime(2020, 01, 01);
            _endDate = new DateTime(2020, 01, 01);

            AmountShouldBe(10);
        }

        [Test]
        public void WhenQueryOneEntireMonth()
        {
            GivenListOfBudgets(new List<Budget>()
                               {
                                   new Budget
                                   {
                                       YearMonth = "202001" ,
                                       Amount    = 310
                                   } ,
                               });
            _repo.GetAll().Returns(_budgets);
            _startDate = new DateTime(2020, 01, 01);
            _endDate = new DateTime(2020, 01, 31);

            AmountShouldBe(310);
        }

        [Test]
        public void WhenQueryCrossEntireTwoMonth()
        {
            GivenListOfBudgets(new List<Budget>()
                               {
                                   new Budget
                                   {
                                       YearMonth = "202001" ,
                                       Amount    = 310
                                   } ,
                                   new Budget
                                   {
                                       YearMonth = "202002" ,
                                       Amount    = 290
                                   }
                               });
            _repo.GetAll().Returns(_budgets);
            _startDate = new DateTime(2020, 01, 01);
            _endDate = new DateTime(2020, 02, 29);

            AmountShouldBe(600);
        }


        [Test]
        public void WhenQueryCrossTwoMonth()
        {
            GivenListOfBudgets(new List<Budget>()
                               {
                                   new Budget
                                   {
                                       YearMonth = "202001" ,
                                       Amount    = 310
                                   } ,
                                   new Budget
                                   {
                                       YearMonth = "202002" ,
                                       Amount    = 2900
                                   }
                               });
            _repo.GetAll().Returns(_budgets);
            _startDate = new DateTime(2020, 01, 31);
            _endDate = new DateTime(2020, 02, 2);

            AmountShouldBe(210);
        }

        [Test]
        public void WhenQueryCrossThreeMonth()
        {
            GivenListOfBudgets(new List<Budget>()
                               {
                                   new Budget
                                   {
                                       YearMonth = "202001" ,
                                       Amount    = 310
                                   } ,
                                   new Budget
                                   {
                                       YearMonth = "202002" ,
                                       Amount    = 2900
                                   } ,
                                   new Budget
                                   {
                                       YearMonth = "202003" ,
                                       Amount    = 31
                                   },
                                   new Budget
                                   {
                                       YearMonth = "200001" ,
                                       Amount    = 31
                                   }
                               });
            _repo.GetAll().Returns(_budgets);
            _startDate = new DateTime(2020, 01, 31);
            _endDate = new DateTime(2020, 03, 01);

            AmountShouldBe(2911);
        }

        [Test]
        public void WhenQueryCrossThreeMonthByMiddleNoBudget()
        {
            GivenListOfBudgets(new List<Budget>()
                               {
                                   new Budget
                                   {
                                       YearMonth = "202001" ,
                                       Amount    = 310
                                   } ,
                                   new Budget
                                   {
                                       YearMonth = "202003" ,
                                       Amount    = 31
                                   },
                                   new Budget
                                   {
                                       YearMonth = "200001" ,
                                       Amount    = 31
                                   }
                               });
            _repo.GetAll().Returns(_budgets);
            _startDate = new DateTime(2020, 01, 31);
            _endDate = new DateTime(2020, 03, 01);

            AmountShouldBe(11);
        }


        [Test]
        public void WhenEmptyBudget()
        {
            GivenListOfBudgets(new List<Budget>()
                               {
                                  new Budget()
                                  {
                                      YearMonth = "208001",
                                      Amount = 100,
                                  }
                               });
            _repo.GetAll().Returns(_budgets);
            _startDate = new DateTime(2020, 01, 31);
            _endDate = new DateTime(2020, 03, 01);

            AmountShouldBe(0);
        }

        private void AmountShouldBe(double expected)
        {
            var amount = _budgetService.Query(_startDate, _endDate);
            Assert.AreEqual(expected, amount);
        }
    }
}