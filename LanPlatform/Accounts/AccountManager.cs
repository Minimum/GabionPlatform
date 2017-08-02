using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using GabionPlatform.Auth;
using GabionPlatform.DAL;
using GabionPlatform.Engine;
using GabionPlatform.Events;
using GabionPlatform.Models;
using GabionPlatform.Models.Requests;
using GabionPlatform.Network;
using GabionPlatform.Network.Messages;
using GabionPlatform.Settings;

namespace GabionPlatform.Accounts
{
    public class AccountManager
    {
        // Access flags
        public const String FlagViewHiddenAccount = "AccountViewHidden";
        public const String FlagCreateAccount = "AccountCreate";
        public const String FlagEditAccountBasic = "AccountEditBasic";
        public const String FlagEditAccountAdvanced = "AccountEditAdvanced";
        public const String FlagEditUsername = "AccountAuthUsernameEdit";
        public const String FlagEditSession = "AccountAuthSessionEdit";

        // Settings
        public const String SettingAllowRegister = "AccountAllowRegister";
        public const String SettingDefaultAvatar = "AccountDefaultAvatar";

        protected PlatformContext Context;
        protected UserAccount LocalAccount;

        protected AppInstance Instance;

        public AccountManager(AppInstance instance)
        {
            Context = instance.Context;
            LocalAccount = null;

            Instance = instance;
        }

        public void Install()
        {
            SettingsManager settings = Instance.Settings;

            settings.AddSetting(new PlatformSetting(SettingAllowRegister, "Allow Account Registrations",
                "Allows new users to register.", "0"));

            settings.AddSetting(new PlatformSetting(SettingDefaultAvatar, "Default Avatar", "The default avatar for users.", "0"));

            AccessRecord accessRecord = Context.AccessRecord.SingleOrDefault(s => s.Id == 0);
            UserAccount account = Context.Account.SingleOrDefault(s => s.Id == 0);
            AccountEditField editField = Context.AccountEditField.SingleOrDefault(s => s.Id == 0);
            AccountEditRecord editRecord = Context.AccountEditRecord.SingleOrDefault(s => s.Id == 0);
            UserRoleAccess accountRole = Context.AccountRole.SingleOrDefault(s => s.Id == 0);
            AuthSession session = Context.AuthSession.SingleOrDefault(s => s.Id == 0);
            AuthSessionAttempt sessionAttempt = Context.AuthSessionAttempt.SingleOrDefault(s => s.Id == 0);
            AuthUsername username = Context.AuthUsername.SingleOrDefault(s => s.Id == 0);
            AuthUsernameAttempt usernameAttempt = Context.AuthUsernameAttempt.SingleOrDefault(s => s.Id == 0);
            UserRole role = Context.Role.SingleOrDefault(s => s.Id == 0);

            return;
        }

        public UserAccount AuthenticateLocalUser()
        {
            UserAccount localAccount = null;

            // Get session cookies
            HttpCookie sessionId = HttpContext.Current.Request.Cookies["LPSessionId"];
            HttpCookie sessionKey = HttpContext.Current.Request.Cookies["LPSessionKey"];

            if (sessionId != null && sessionKey != null)
            {
                // Get session id
                long id = AuthSession.GetIdFromCookie(sessionId);

                // Authenticate session
                localAccount = AuthBySession(id, sessionKey.Value);

                if (localAccount != null)
                {
                    // Mark user as active if previously inactive
                    if (EngineUtil.CurrentTime >= localAccount.LastActive + 1800)
                    {
                        NetMessageManager.AddMessageBroadcastQuick(Instance, new NewActiveUserMessage(localAccount.Id));
                    }

                    // Update account's last active time
                    UpdateActivity(localAccount);
                }
                else
                {
                    // Log invalid session key attempt
                    Context.AuthSessionAttempt.Add(new AuthSessionAttempt(Instance, id, sessionKey.Value));
                }
            }

            // Set local account
            LocalAccount = localAccount;

            // Save changes
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                // If concurrency issue is detected, default to database
                e.Entries.Single().Reload();
            }

            return localAccount;
        }

        public UserAccount AuthByUsername(String username, String password)
        {
            UserAccount account = null;
            AuthUsername auth = GetUsername(username);

            // Authenticate
            if(auth != null && auth.Authenticate(password))
            {
                // Load account
                account = GetAccount(auth.Account);
            }

            if (account != null)
            {
                PostAuthTasks(account);
            }

            return account;
        }

        public AuthUsername GetUsername(String username)
        {
            return Context.AuthUsername.SingleOrDefault(s => s.Username.Equals(username, StringComparison.Ordinal));
        }

        public List<AuthUsername> GetUsernames(UserAccount account)
        {
            return Context.AuthUsername.Where(s => s.Account == account.Id).ToList();
        }

        public AuthUsername CreateUsername(UserAccount account, String username, String password)
        {
            return CreateUsername(account.Id, username, password);
        }

        public AuthUsername CreateUsername(long accountId, String username, String password)
        {
            AuthUsername auth = null;

            // Check if username already exists
            if (GetUsername(username) == null)
            {
                // Create new auth
                auth = AuthUsername.CreateAuth(username, password);

                auth.Account = accountId;

                Context.AuthUsername.Add(auth);
            }

            return auth;
        }

        public void RemoveUsername(AuthUsername username)
        {
            Context.AuthUsername.Remove(username);

            return;
        }

        public UserAccount AuthBySession(long sessionId, String key)
        {
            UserAccount account = null;
            AuthSession auth = GetSession(sessionId);

            // Authenticate
            if (auth != null && auth.Authenticate(sessionId, key))
            {
                // Load account
                account = GetAccount(auth.Account);
            }

            if (account != null)
            {
                PostAuthTasks(account);
            }

            return account;
        }

        public AuthSession GetSession(long id)
        {
            return Context.AuthSession.SingleOrDefault(s => s.Id == id);
        }

        public AuthSession CreateSession(UserAccount account)
        {
            AuthSession session = new AuthSession();

            session.Account = account.Id;
            session.Key = AuthSession.GenerateKey();

            Context.AuthSession.Add(session);

            return session;
        }

        public void RemoveSession(AuthSession session)
        {
            Context.AuthSession.Remove(session);

            return;
        }

        public void RemoveSession(long id)
        {
            AuthSession session = GetSession(id);

            if (session != null)
            {
                Context.AuthSession.Remove(session);
            }

            return;
        }

        public void PostAuthTasks(UserAccount account)
        {
            LanEventManager.PostAuthTasks(account, Instance);

            return;
        }

        public UserAccount GetAccount(long id)
        {
            return Context.Account.SingleOrDefault(s => s.Id == id);
        }

        public UserAccount GetAccountReadOnly(long id)
        {
            return Context.Account.Where(s => s.Id == id).AsNoTracking().SingleOrDefault();
        }

        public UserAccount GetAccountByUrl(String url)
        {
            UserAccount account = null;

            if (url.Length > 0)
            {
                account = Context.Account.SingleOrDefault(s => s.CustomUrl.Equals(url, StringComparison.OrdinalIgnoreCase));
            }

            return account;
        }

        public List<UserAccount> GetActiveAccounts(long deltaTime, int numAccounts, int startPos)
        {
            List<UserAccount> accounts;

            if (LocalAccount != null)
            {
                if (CheckAccess(LocalAccount, FlagViewHiddenAccount, false))
                {
                    accounts =
                    Context.Account.Where(
                        s =>
                            s.LastActive >= EngineUtil.CurrentTime - deltaTime)
                        .OrderByDescending(s => s.LastActive)
                        .Skip(startPos)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
                }
                else
                {
                    accounts =
                    Context.Account.Where(
                        s =>
                            s.LastActive >= EngineUtil.CurrentTime - deltaTime &&
                            s.Visibility != AccountVisibility.HiddenFromUsers)
                        .OrderByDescending(s => s.LastActive)
                        .Skip(startPos)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
                }
            }
            else
            {
                accounts =
                    Context.Account.Where(
                        s =>
                            s.LastActive >= EngineUtil.CurrentTime - deltaTime &&
                            s.Visibility == AccountVisibility.Visible)
                        .OrderByDescending(s => s.LastActive)
                        .Skip(startPos)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
            }

            return accounts;
        }

        public List<UserAccount> GetInactiveAccounts(long deltaTime, int numAccounts, int startPos)
        {
            List<UserAccount> accounts;

            if (LocalAccount != null)
            {
                if (CheckAccess(LocalAccount, FlagViewHiddenAccount, false))
                {
                    accounts =
                    Context.Account.Where(
                        s =>
                            s.LastActive < EngineUtil.CurrentTime - deltaTime)
                        .OrderByDescending(s => s.LastActive)
                        .Skip(startPos)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
                }
                else
                {
                    accounts =
                    Context.Account.Where(
                        s =>
                            s.LastActive < EngineUtil.CurrentTime - deltaTime &&
                            s.Visibility != AccountVisibility.HiddenFromUsers)
                        .OrderByDescending(s => s.LastActive)
                        .Skip(startPos)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
                }
            }
            else
            {
                accounts =
                    Context.Account.Where(
                        s =>
                            s.LastActive < EngineUtil.CurrentTime - deltaTime &&
                            s.Visibility == AccountVisibility.Visible)
                        .OrderByDescending(s => s.LastActive)
                        .Skip(startPos)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
            }

            return accounts;
        }

        public List<UserAccount> GetAccountSearch(SearchAccountRequest request)
        {
            request.SanityCheck();

            long startPos = (request.Page - 1) * request.PageSize;

            IQueryable<UserAccount> query;

            // Only show accounts visible to user
            if (LocalAccount != null)
            {
                if (CheckAccess(LocalAccount, FlagViewHiddenAccount, false))
                {
                    query = Context.Account.Where(s => s.Id > 0);
                }
                else
                {
                    query = Context.Account.Where(s => s.Visibility != AccountVisibility.HiddenFromUsers);
                }
            }
            else
            {
                query = Context.Account.Where(s => s.Visibility == AccountVisibility.Visible);
            }

            // Sort
            switch (request.SortBy)
            {
                case SearchAccountSort.DisplayName:
                {
                    query = request.SortDescending ? query.OrderByDescending(s => s.DisplayName).ThenBy(s => s.Id) : query.OrderBy(s => s.DisplayName).ThenBy(s => s.Id);

                    break;
                }

                case SearchAccountSort.FirstName:
                {
                    query = request.SortDescending ? query.OrderByDescending(s => s.FirstName).ThenBy(s => s.Id) : query.OrderBy(s => s.FirstName).ThenBy(s => s.Id);

                    break;
                }

                case SearchAccountSort.LastName:
                {
                    query = request.SortDescending ? query.OrderByDescending(s => s.LastName).ThenBy(s => s.Id) : query.OrderBy(s => s.LastName).ThenBy(s => s.Id);

                    break;
                }

                case SearchAccountSort.LastActive:
                {
                    query = request.SortDescending ? query.OrderByDescending(s => s.LastActive).ThenBy(s => s.Id) : query.OrderBy(s => s.LastActive).ThenBy(s => s.Id);

                    break;
                }

                case SearchAccountSort.TotalLans:
                {
                    query = request.SortDescending ? query.OrderByDescending(s => s.TotalEvents).ThenBy(s => s.Id) : query.OrderBy(s => s.TotalEvents).ThenBy(s => s.Id);

                    break;
                }

                default:
                {
                    query = request.SortDescending ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id);

                    break;
                }
            }

            while (startPos > Int32.MaxValue)
            {
                query = query.Skip(Int32.MaxValue);

                startPos -= Int32.MaxValue;
            }

            return query.Skip((int) startPos).Take(request.PageSize).AsNoTracking().ToList();
        }

        public List<UserAccount> GetAccountListing(long startPos, int numAccounts)
        {
            List<UserAccount> accounts;

            if (LocalAccount != null)
            {
                if (CheckAccess(LocalAccount, FlagViewHiddenAccount, false))
                {
                    accounts =
                    Context.Account.Where(
                        s =>
                            s.Id >= startPos)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
                }
                else
                {
                    accounts =
                    Context.Account.Where(
                        s =>
                            s.Id >= startPos &&
                            s.Visibility != AccountVisibility.HiddenFromUsers)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
                }
            }
            else
            {
                accounts =
                    Context.Account.Where(
                        s =>
                            s.Id >= startPos &&
                            s.Visibility == AccountVisibility.Visible)
                        .Take(numAccounts)
                        .AsNoTracking()
                        .ToList();
            }

            return accounts;
        }

        public long GetAccountTotal()
        {
            return Context.Account.LongCount();
        }

        public List<UserAccount> GetAccounts(long startPos, int numAccounts)
        {
            return Context.Account.Where(s => s.Id >= startPos).Take(numAccounts).ToList();
        }

        public void AddAccount(UserAccount account)
        {
            Context.Account.Add(account);

            return;
        }

        /*
         * User Access
         */

        public bool CheckAccess(UserAccount account, String flag, String scope)
        {
            return CheckAccess(account, flag, scope, true);
        }

        public bool CheckAccess(UserAccount account, String flag, String scope, bool record)
        {
            bool success = account.Root || CheckAdminAccess(account.Id, flag, scope);

            if(record)
                AddAccessRecord(account, flag, scope, success);

            return success;
        }

        public bool CheckAccess(UserAccount account, String flag)
        {
            return CheckAccess(account, flag, "platform", true);
        }

        public bool CheckAccess(UserAccount account, String flag, bool record)
        {
            return CheckAccess(account, flag, "platform", record);
        }

        // Check the database for admin flags relating to an account ID
        protected bool CheckAdminAccess(long accountId, String flag, String scope)
        {
            bool success = Context.RoleFlag
                // Role Flag Table
                            .Where(roleFlag => flag.Equals(roleFlag.Flag, StringComparison.OrdinalIgnoreCase) &&
                                scope.Equals(roleFlag.Scope, StringComparison.OrdinalIgnoreCase) &&
                                // Role Table
                                Context.Role.Where(role =>
                                    // Account Role Table
                                    Context.AccountRole.Where(accountRole => accountRole.User == accountId)
                                    .Select(accountRole => accountRole.Role).Contains(role.Id))
                                .Select(role => role.Id).Contains(roleFlag.Role)
                            ).AsNoTracking().FirstOrDefault() != null;

            return success;
        }

        public List<UserRole> GetRolesByAccount(UserAccount account)
        {
            return GetRolesByAccount(account.Id);
        }

        public List<UserRole> GetRolesByAccount(long accountId)
        {
            return Context.Role.Where(role => Context.AccountRole.Where(accountRole => accountRole.User == accountId).Select(accountRole => accountRole.Role).Contains(role.Id)).ToList();
        }

        public UserRole GetRoleById(long id)
        {
            return Context.Role.SingleOrDefault(s => s.Id == id);
        }

        public UserRole GetRoleByName(String name)
        {
            return Context.Role.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<UserPermission> GetAllAccountFlags(UserAccount account)
        {
            return GetAllAccountFlags(account.Id);
        }

        public List<UserPermission> GetAllAccountFlags(long accountId)
        {
            List<UserPermission> accountFlags =
                Context.RoleFlag.Where(
                    flag =>
                        Context.AccountRole.Where(
                            role => role.User == accountId).Select(
                                role => role.Role).Contains(flag.Id))
                    .ToList();

            return accountFlags;
        }

        public void UpdateActivity(UserAccount account)
        {
            account.LastActive = EngineUtil.CurrentTime;

            // Attempt to save last active time, not important if fails to concurrency
            /*try
            {
                Save();
            }
            catch (Exception e) { }*/

            return;
        }

        public void AddAccessRecord(UserAccount account, String flag, String scope, bool success)
        {
            AccessRecord record = new AccessRecord();

            record.Account = account.Id;
            record.Time = EngineUtil.CurrentTime;
            record.Success = success;
            record.Flag = flag;
            record.Scope = scope;

            AddAccessRecord(record);

            return;
        }

        public void AddAccessRecord(AccessRecord record)
        {
            Context.AccessRecord.Add(record);

            return;
        }

        public void Dispose()
        {
            Context.Dispose();

            Context = null;
            LocalAccount = null;

            return;
        }
    }
}