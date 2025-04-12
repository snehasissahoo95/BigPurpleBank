using BigPurpleBank.Application.Enums;
using BigPurpleBank.Application.Interfaces;
using BigPurpleBank.Application.Services;
using BigPurpleBank.Data;
using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace BigPurpleBank.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private Mock<IAccountQueryService> _mockQueryService;
        private AccountsDbContext _context;
        private AccountService _accountService;

        [SetUp]
        public void Setup()
        {
            // In-memory database for mocking DbSet structure (not persistent)
            var options = new DbContextOptionsBuilder<AccountsDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AccountsDb")
                .Options;

            _context = new AccountsDbContext(options);

            _context.Accounts.AddRange(new List<Account>
            {
                new Account
                {
                    ProductCategory = ProductCategory.BUSINESS_LOANS,
                    OpenStatus = OpenStatus.OPEN,
                    IsOwned = true,
                    AccountOwnership = "Individual",
                    DisplayName = "Business Loan Account",
                    MaskedNumber = "****5678",
                    Nickname = "BizLoan",
                    ProductName = "Business Loan Plus"
                },
                new Account
                {
                    ProductCategory = ProductCategory.CRED_AND_CHRG_CARDS,
                    OpenStatus = OpenStatus.CLOSED,
                    IsOwned = false,
                    AccountOwnership = "Joint",
                    DisplayName = "Credit Card Account",
                    MaskedNumber = "****1234",
                    Nickname = "CreditMaster",
                    ProductName = "Credit Card Premium"
                },
                new Account
                {
                    ProductCategory = ProductCategory.TRAVEL_CARDS,
                    OpenStatus = OpenStatus.CLOSED,
                    IsOwned = true,
                    AccountOwnership = "Individual",
                    DisplayName = "Travel Card Account",
                    MaskedNumber = "****4321",
                    Nickname = "TravelMaster",
                    ProductName = "Travel Card Elite"
                }
            });

            _context.SaveChanges();

            _mockQueryService = new Mock<IAccountQueryService>();
            _accountService = new AccountService(_context, _mockQueryService.Object);
        }

        [Test]
        public async Task GetAccountsAsync_AppliesFilterCorrectly()
        {
            var filter = new AccountFilter { IsOwned = true };
            var allAccounts = _context.Accounts.AsQueryable();
            var filteredAccounts = allAccounts.Where(a => a.IsOwned == true);

            _mockQueryService
                .Setup(s => s.ApplyFilter(It.IsAny<IQueryable<Account>>(), filter))
                .Returns(filteredAccounts);

            var result = await _accountService.GetAccountsAsync(filter);

            Assert.That(result.All(a => a.IsOwned == true));
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAccountsAsync_FilterByProductCategory_ReturnsCorrectResults()
        {
            var filter = new AccountFilter { ProductCategory = ProductCategory.BUSINESS_LOANS };
            var allAccounts = _context.Accounts.AsQueryable();
            var filteredAccounts = allAccounts.Where(a => a.ProductCategory == ProductCategory.BUSINESS_LOANS);


            _mockQueryService
                .Setup(s => s.ApplyFilter(It.IsAny<IQueryable<Account>>(), filter))
                .Returns(filteredAccounts);

            var result = await _accountService.GetAccountsAsync(filter);

            Assert.That(result.All(a => a.ProductCategory == ProductCategory.BUSINESS_LOANS));
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}