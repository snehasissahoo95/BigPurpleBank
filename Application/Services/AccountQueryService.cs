using BigPurpleBank.Application.Enums;
using BigPurpleBank.Application.Interfaces;
using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;

namespace BigPurpleBank.Application.Services
{
    public class AccountQueryService : IAccountQueryService
    {
        private readonly ILogger<AccountQueryService> _logger;

        public AccountQueryService(ILogger<AccountQueryService> logger)
        {
            _logger = logger;
        }

        public IQueryable<Account> ApplyFilter(IQueryable<Account> query, AccountFilter filter)
        {
            try
            {
                // Validate filter values
                if (filter.ProductCategory.HasValue && !Enum.IsDefined(typeof(ProductCategory), filter.ProductCategory.Value))
                {
                    throw new ArgumentException($"Invalid ProductCategory value: {filter.ProductCategory.Value}");
                }

                if (filter.ProductCategory.HasValue)
                {
                    query = query.Where(a => a.ProductCategory == filter.ProductCategory.Value);
                }

                if (filter.OpenStatus != OpenStatus.ALL)
                {
                    query = query.Where(a => a.OpenStatus == filter.OpenStatus);
                }

                if (filter.IsOwned.HasValue)
                {
                    query = query.Where(a => a.IsOwned == filter.IsOwned.Value);
                }

                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying filters in AccountQueryService.");
                throw new ApplicationException("Failed to apply account filters.", ex);
            }
        }
    }
}