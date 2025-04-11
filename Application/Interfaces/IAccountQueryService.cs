using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;

namespace BigPurpleBank.Application.Interfaces
{
    public interface IAccountQueryService
    {
        IQueryable<Account> ApplyFilter(IQueryable<Account> query, AccountFilter filter);
    }
}
