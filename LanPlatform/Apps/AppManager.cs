using System;
using System.Collections.Generic;
using System.Linq;
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
                account.Apps =
                    Context.App.Where(
                        app =>
                            Context.LoanerApp.Where(loanerApp => loanerApp.Account == account.Id)
                                .Select(loanerApp => loanerApp.App)
                                .Contains(app.Id)).ToList();
            }

            return accounts;
        }

        public LoanerAccount GetLoanerAccountById(long id)
        {
            LoanerAccount account = Context.LoanerAccount.FirstOrDefault(s => s.Id == id);

            // Load apps
            if (account != null)
            {
                account.Apps =
                    Context.App.Where(
                        app =>
                            Context.LoanerApp.Where(loanerApp => loanerApp.Account == account.Id)
                                .Select(loanerApp => loanerApp.App)
                                .Contains(app.Id)).ToList();
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
                account.Apps =
                    Context.App.Where(
                        app =>
                            Context.LoanerApp.Where(loanerApp => loanerApp.Account == account.Id)
                                .Select(loanerApp => loanerApp.App)
                                .Contains(app.Id)).ToList();
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

        public LoanerApp GetLoanerApp(LoanerAccount account, long appId)
        {
            return GetLoanerApp(account.Id, appId);
        }

        public LoanerApp GetLoanerApp(long accountId, long appId)
        {
            return Context.LoanerApp.FirstOrDefault(s => s.Account == accountId && s.App == appId);
        }

        public void AddLoanerApp(LoanerApp app)
        {
            Context.LoanerApp.Add(app);

            return;
        }

        public void RemoveLoanerApp(LoanerApp app)
        {
            Context.LoanerApp.Remove(app);

            return;
        }

        public App GetAppById(long id)
        {
            return Context.App.FirstOrDefault(s => s.Id == id);
        }

        public List<App> GetApps()
        {
            return Context.App.Where(s => s.Id > 0).ToList();
        }

        public void AddApp(App app)
        {
            Context.App.Add(app);

            return;
        }

        public void RemoveApp(App app)
        {
            Context.App.Remove(app);

            return;
        }
    }
}