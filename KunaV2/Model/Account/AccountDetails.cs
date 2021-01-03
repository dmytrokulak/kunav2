using System.Collections.Generic;

namespace KunaV2.Model.Account
{
    public class AccountDetails
    {
        public string Email { get; set; }
        public bool Activated { get; set; }
        public IList<Wallet> Accounts { get; set; }
    }
}
