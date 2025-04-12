using BigPurpleBank.Application.Enums;
using BigPurpleBank.Application.Services;
using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;
using NUnit.Framework;

namespace BigPurpleBank.Tests
{
    [TestFixture]
    public class AccountQueryServiceTests
    {
        private AccountQueryService _queryService;
        private IQueryable<Account> _accounts;

        [SetUp]
        public void SetUp()
        {
            _queryService = new AccountQueryService();
            _accounts = new List<Account>
            {
                new Account { ProductCategory = ProductCategory.BUSINESS_LOANS, OpenStatus = OpenStatus.OPEN, IsOwned = true },
                new Account { ProductCategory = ProductCategory.CRED_AND_CHRG_CARDS, OpenStatus = OpenStatus.CLOSED, IsOwned = false },
                new Account { ProductCategory = ProductCategory.TRAVEL_CARDS, OpenStatus = OpenStatus.CLOSED, IsOwned = true }
            }.AsQueryable();
        }

        [Test]
        public void ApplyFilter_FilterByProductCategory_ReturnsCorrectResults()
        {
            var filter = new AccountFilter { ProductCategory = ProductCategory.BUSINESS_LOANS };
            var result = _queryService.ApplyFilter(_accounts, filter);

            Assert.That(result.All(a => a.ProductCategory == ProductCategory.BUSINESS_LOANS));
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ApplyFilter_FilterByOpenStatus_ReturnsCorrectResults()
        {
            var filter = new AccountFilter { OpenStatus = OpenStatus.CLOSED };
            var result = _queryService.ApplyFilter(_accounts, filter);

            Assert.That(result.All(a => a.OpenStatus == OpenStatus.CLOSED));
        }

        [Test]
        public void ApplyFilter_FilterByOwnership_ReturnsCorrectResults()
        {
            var filter = new AccountFilter { IsOwned = true };
            var result = _queryService.ApplyFilter(_accounts, filter);

            Assert.That(result.All(a => a.IsOwned == true));
        }
    }
}
