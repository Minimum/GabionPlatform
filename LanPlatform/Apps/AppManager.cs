using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Accounts;
using GabionPlatform.DAL;
using GabionPlatform.Models;

namespace GabionPlatform.Apps
{
    public class AppManager
    {
        public const String FlagAppEdit = "AppEdit";
        public const String FlagLoanerEdit = "AppLoanerEdit";
        public const String FlagLoanerCheckout = "AppLoanerCheckout";
        public const String FlagLoanerSteamCode = "AppLoanerSteamCode";

        protected PlatformContext Context;

        protected AppInstance Instance;

        public AppManager(AppInstance instance)
        {
            Context = instance.Context;

            Instance = instance;
        }

        public void Install()
        {
            
        }

        public List<LoanerAccount> GetLoanerAccounts()
        {
            List<LoanerAccount> accounts = Context.LoanerAccount.Where(s => s.Id > 0).ToList();

            // Load apps
            foreach (LoanerAccount account in accounts)
            {
                account.Apps = Context.LoanerApp.Where(s => s.Account == account.Id).ToList();
            }

            return accounts;
        }

        public LoanerAccount GetLoanerAccountById(long id)
        {
            LoanerAccount account = Context.LoanerAccount.FirstOrDefault(s => s.Id == id);

            // Load apps
            if (account != null)
            {
                account.Apps = Context.LoanerApp.Where(s => s.Account == account.Id).ToList();
            }

            return account;
        }

        public LoanerAccount GetLoanerAccountByName(String username)
        {
            LoanerAccount account =
                Context.LoanerAccount.FirstOrDefault(
                    s => s.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            // Load apps
            if (account != null)
            {
                account.Apps = Context.LoanerApp.Where(s => s.Account == account.Id).ToList();
            }

            return account;
        }

        public int GetUserCheckoutCount(UserAccount user)
        {
            return Context.LoanerAccount.Count(s => s.CheckoutUser == user.Id);
        }

        public void AddLoanerAccount(LoanerAccount account)
        {
            Context.LoanerAccount.Add(account);

            return;
        }

        public List<App> GetApps()
        {
            return Context.App.Where(s => s.Id > 0).ToList();
        }
    }
}