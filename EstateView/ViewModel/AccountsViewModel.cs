using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using EstateView.Core.Model;

namespace EstateView.ViewModel
{
    public class AccountsViewModel : ViewModel
    {
        public AccountsViewModel(EstateProjectionAccountBook accounts)
        {
            this.Bind(accounts);
        }

        public IEnumerable<Account> Accounts
        {
            get { return this.GetValue(() => this.Accounts); }
            set { this.SetValue(() => this.Accounts, value); }
        }

        public Account SelectedAccount
        {
            get { return this.GetValue(() => this.SelectedAccount); }
            set { this.SetValue(() => this.SelectedAccount, value); }
        }

        public void Bind(EstateProjectionAccountBook accounts)
        {
            this.Accounts = accounts.Where(account => account.Transactions.Any());
            
            if (this.SelectedAccount != null)
            {
                this.SelectedAccount = accounts.FirstOrDefault(a => a.Name == this.SelectedAccount.Name);
            }
        }
    }
}
