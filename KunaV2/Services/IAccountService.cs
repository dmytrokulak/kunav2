using KunaV2.Model.Account;
using System.Threading.Tasks;

namespace KunaV2.Services
{
    public interface IAccountService
    {
        Task<AccountDetails> GetDetailsAsync();
    }
}
