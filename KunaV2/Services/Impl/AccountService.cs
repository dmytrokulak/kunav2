using KunaV2.Model.Account;
using RestSharp;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KunaV2.Services.Impl
{
    public class AccountService : ServiceBase, IAccountService
    {
        public async Task<AccountDetails> GetDetailsAsync()
        {
            return await ExecuteRequestWithRetry<AccountDetails>(Method.GET, Urls.Account);
        }

        public AccountService(ILogger<ServiceBase> logger, IRestClient client) : base(logger, client)
        {
        }
    }
}