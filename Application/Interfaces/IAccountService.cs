using BigPurpleBank.Model.DTOs;
using BigPurpleBank.Model.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace BigPurpleBank.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IQueryable<Account>> GetAccountsAsync(AccountFilter filter);
    }
}
