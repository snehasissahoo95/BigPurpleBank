using BigPurpleBank.Application.Enums;
using BigPurpleBank.Application.Interfaces;
using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;

namespace BigPurpleBank.Application.Services
{
    public class AccountQueryService : IAccountQueryService
    {
        public IQueryable<Account> ApplyFilter(IQueryable<Account> query, AccountFilter filter)
        {
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
    }
}
