using BigPurpleBank.Application.Enums;
using BigPurpleBank.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace BigPurpleBank.Data
{
    public class AccountsDbContext : DbContext
    {
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .Property(a => a.ProductCategory)
                .HasConversion<string>();

            modelBuilder.Entity<Account>()
                .Property(a => a.OpenStatus)
                .HasConversion<string>();

            // Seed Data
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CreationDate = new DateTime(2022, 06, 15),
                    DisplayName = "My Business Loan",
                    Nickname = "BizLoan",
                    OpenStatus = OpenStatus.CLOSED,
                    IsOwned = true,
                    AccountOwnership = "UNKNOWN",
                    MaskedNumber = "1234****5678",
                    ProductCategory = ProductCategory.BUSINESS_LOANS,
                    ProductName = "Business Loan Platinum"
                },
                new Account
                {
                    AccountId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    CreationDate = new DateTime(2021, 01, 10),
                    DisplayName = "Travel Expense Card",
                    Nickname = "TravelCard",
                    OpenStatus = OpenStatus.OPEN,
                    IsOwned = true,
                    AccountOwnership = "SOLE",
                    MaskedNumber = "2345****6789",
                    ProductCategory = ProductCategory.TRAVEL_CARDS,
                    ProductName = "Travel Plus"
                },
                new Account
                {
                    AccountId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    CreationDate = new DateTime(2020, 08, 01),
                    DisplayName = "Home Mortgage",
                    Nickname = "Mortgage",
                    OpenStatus = OpenStatus.OPEN,
                    IsOwned = false,
                    AccountOwnership = "JOINT",
                    MaskedNumber = "3456****7890",
                    ProductCategory = ProductCategory.RESIDENTIAL_MORTGAGES,
                    ProductName = "Smart Home Loan"
                }
            );
        }
    }
}
