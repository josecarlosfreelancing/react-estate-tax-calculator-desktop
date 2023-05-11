using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EstateView.Core.Model
{
    public abstract class AccountBook : IEnumerable<Account>
    {
        private readonly Dictionary<string, Account> accounts;
        
        protected AccountBook()
        {
            this.accounts = new Dictionary<string, Account>();
        }

        public int GetMinYear()
        {
            return this.accounts.Values.SelectMany(account => account.Transactions).Min(transaction => transaction.Year);
        }

        public int GetMaxYear()
        {
            return this.accounts.Values.SelectMany(account => account.Transactions).Max(transaction => transaction.Year);
        }

        public IEnumerator<Account> GetEnumerator()
        {
            return this.accounts.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected Account RegisterAccount(string name)
        {
            Account account = new Account(name);
            this.accounts.Add(name, account);
            return account;
        }
    }
}
