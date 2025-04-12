using BigPurpleBank.Application.Interfaces;
using BigPurpleBank.Data;
using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BigPurpleBank.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountsDbContext _context;
        private readonly IAccountQueryService _accountQueryService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            AccountsDbContext context,
            IAccountQueryService accountQueryService,
            ILogger<AccountService> logger)
        {
            _context = context;
            _accountQueryService = accountQueryService;
            _logger = logger;
        }

        public async Task<IQueryable<Account>> GetAccountsAsync(AccountFilter filter)
        {
            try
            {
                var query = _context.Accounts.AsQueryable();
                query = _accountQueryService.ApplyFilter(query, filter);

                // Optional: validate that the filtered query won't fail on enumeration later
                await query.Take(1).ToListAsync();

                return query;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid filter argument provided in GetAccountsAsync.");
                throw new ApplicationException("Invalid filter argument.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database access error in GetAccountsAsync.");
                throw new ApplicationException("A database error occurred.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred in GetAccountsAsync.");
                throw new ApplicationException("An unexpected error occurred while retrieving accounts.", ex);
            }
        }
    }
}