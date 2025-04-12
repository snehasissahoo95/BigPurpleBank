using BigPurpleBank.Application.Interfaces;
using BigPurpleBank.Data;
using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;

namespace BigPurpleBank.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountsDbContext _context;
        private readonly IAccountQueryService _accountQueryService;

        public AccountService(AccountsDbContext context, IAccountQueryService accountQueryService)
        {
            _context = context;
            _accountQueryService = accountQueryService;
        }

        // Get accounts with filtering
        public async Task<IQueryable<Account>> GetAccountsAsync(AccountFilter filter)
        {
            var query = _context.Accounts.AsQueryable();
            query = _accountQueryService.ApplyFilter(query, filter);

            return query;
        }
    }
}
