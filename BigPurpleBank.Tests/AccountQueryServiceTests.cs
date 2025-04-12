using BigPurpleBank.Application.Enums;
using BigPurpleBank.Application.Services;
using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;
using Moq;
using NUnit.Framework;

namespace BigPurpleBank.Tests
{
    [TestFixture]
    public class AccountQueryServiceTests
    {
        private Mock<ILogger<AccountQueryService>> _mockLogger;
        private AccountQueryService _queryService;
        private IQueryable<Account> _accounts;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<AccountQueryService>>();
            _queryService = new AccountQueryService(_mockLogger.Object);

            _accounts = new[]
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
            var filter = new AccountFilter { OpenStatus = OpenStatus.OPEN };

            var result = _queryService.ApplyFilter(_accounts, filter);

            Assert.That(result.All(a => a.OpenStatus == OpenStatus.OPEN));
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ApplyFilter_FilterByOwnership_ReturnsCorrectResults()
        {
            var filter = new AccountFilter { IsOwned = true };

            var result = _queryService.ApplyFilter(_accounts, filter);

            Assert.That(result.All(a => a.IsOwned == true));
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ApplyFilter_ErrorOccurs_LogsErrorAndThrowsApplicationException()
        {
            var filter = new AccountFilter { ProductCategory = (ProductCategory)999 }; // Invalid category to trigger error

            var ex = Assert.Throws<ApplicationException>(() => _queryService.ApplyFilter(_accounts, filter));

            Assert.That(ex.Message, Is.EqualTo("Failed to apply account filters."));

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error applying filters in AccountQueryService.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
