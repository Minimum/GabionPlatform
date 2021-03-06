﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using GabionPlatform.Accounts;
using GabionPlatform.Auth;
using GabionPlatform.Content;
using GabionPlatform.DTO.Accounts;
using GabionPlatform.Models;
using GabionPlatform.Models.Requests;
using GabionPlatform.Models.Responses;
using GabionPlatform.Network;
using GabionPlatform.Network.Messages;

namespace GabionPlatform.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {
        // Account specific actions

        [HttpGet]
        [Route("account/{id}")]
        public HttpResponseMessage GetAccountById(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            if (id > 0)
            {
                UserAccount account = instance.Accounts.GetAccountReadOnly(id);

                if (account != null)
                {
                    instance.SetData(new UserAccountDto(account), "UserAccount");
                }
                else
                {
                    instance.Status = AppResponseStatus.ResponseError;
                    instance.StatusCode = "INVALID_ACCOUNT";
                }
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "INVALID_ACCOUNT";
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("url/{url}")]
        public HttpResponseMessage GetAccountByUrl(String url)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount account = instance.Accounts.GetAccountByUrl(url);

            if (account != null)
            {
                instance.SetData(new UserAccountDto(account), "UserAccount");
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "INVALID_ACCOUNT";
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("account/{id}")]
        public HttpResponseMessage EditAccount(long id, [FromBody] UserAccountDto userAccount)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;

            if (localAccount != null && userAccount != null)
            {
                UserAccount targetAccount = instance.Accounts.GetAccount(id);

                if (targetAccount != null)
                {
                    if (userAccount.Id == localAccount.Id || instance.Accounts.CheckAccess(localAccount, AccountManager.FlagEditAccountBasic))
                    {
                        AccountEditRecord editRecord = new AccountEditRecord();

                        // Self editable fields

                        // Gender
                        if (targetAccount.Gender != userAccount.Gender)
                        {
                            editRecord.AddField("Gender", targetAccount.Gender, userAccount.Gender);

                            targetAccount.Gender = userAccount.Gender;
                        }

                        // First Name
                        if (targetAccount.FirstName != userAccount.FirstName)
                        {
                            editRecord.AddField("FirstName", targetAccount.FirstName, userAccount.LastName);

                            targetAccount.FirstName = userAccount.FirstName;
                        }

                        // Last Name
                        if (targetAccount.LastName != userAccount.LastName)
                        {
                            editRecord.AddField("LastName", targetAccount.LastName, userAccount.LastName);

                            targetAccount.LastName = userAccount.LastName;
                        }

                        // Birthday
                        if (targetAccount.Birthday != userAccount.Birthday)
                        {
                            editRecord.AddField("Birthday", targetAccount.Birthday, userAccount.Birthday);

                            targetAccount.Birthday = userAccount.Birthday;
                        }

                        // ContactEmail
                        if (targetAccount.ContactEmail != userAccount.ContactEmail)
                        {
                            editRecord.AddField("ContactEmail", targetAccount.ContactEmail, userAccount.ContactEmail);

                            targetAccount.ContactEmail = userAccount.ContactEmail;
                        }

                        // ContactPhone
                        if (targetAccount.ContactPhone != userAccount.ContactPhone)
                        {
                            editRecord.AddField("ContactPhone", targetAccount.ContactPhone, userAccount.ContactPhone);

                            targetAccount.ContactPhone = userAccount.ContactPhone;
                        }

                        // ContactFacebook
                        if (targetAccount.ContactFacebook != userAccount.ContactFacebook)
                        {
                            editRecord.AddField("ContactFacebook", targetAccount.ContactFacebook, userAccount.ContactFacebook);

                            targetAccount.ContactFacebook = userAccount.ContactFacebook;
                        }

                        // ContactSteam
                        if (targetAccount.ContactSteam != userAccount.ContactSteam)
                        {
                            editRecord.AddField("ContactSteam", targetAccount.ContactSteam, userAccount.ContactSteam);

                            targetAccount.ContactSteam = userAccount.ContactSteam;
                        }

                        // Display Name
                        if (targetAccount.DisplayName != userAccount.DisplayName)
                        {
                            editRecord.AddField("DisplayName", targetAccount.DisplayName, userAccount.DisplayName);

                            targetAccount.DisplayName = userAccount.DisplayName;
                        }

                        // Avatar
                        if (targetAccount.Avatar != userAccount.Avatar)
                        {
                            editRecord.AddField("Avatar", targetAccount.Avatar, userAccount.Avatar);

                            targetAccount.Avatar = userAccount.Avatar;
                        }

                        // Visibility
                        if (targetAccount.Visibility != userAccount.Visibility)
                        {
                            editRecord.AddField("Visibility", targetAccount.Visibility, userAccount.Visibility);

                            targetAccount.Visibility = userAccount.Visibility;
                        }

                        // Admin only editable fields

                        if (instance.Accounts.CheckAccess(localAccount, AccountManager.FlagEditAccountAdvanced))
                        {
                            // AccountType
                            if (targetAccount.AccountType != userAccount.AccountType)
                            {
                                editRecord.AddField("AccountType", targetAccount.AccountType, userAccount.AccountType);

                                targetAccount.AccountType = userAccount.AccountType;
                            }

                            // TotalEvents
                            if (targetAccount.TotalEvents != userAccount.TotalEvents)
                            {
                                editRecord.AddField("TotalEvents", targetAccount.TotalEvents, userAccount.TotalEvents);

                                targetAccount.TotalEvents = userAccount.TotalEvents;
                            }

                            // EventOffset
                            if (targetAccount.EventOffset != userAccount.EventOffset)
                            {
                                editRecord.AddField("EventOffset", targetAccount.EventOffset, userAccount.EventOffset);

                                targetAccount.EventOffset = userAccount.EventOffset;
                            }

                            // RemoteEvents
                            if (targetAccount.RemoteEvents != userAccount.RemoteEvents)
                            {
                                editRecord.AddField("RemoteEvents", targetAccount.RemoteEvents, userAccount.RemoteEvents);

                                targetAccount.RemoteEvents = userAccount.RemoteEvents;
                            }

                            // LastEvent
                            if (targetAccount.LastEvent != userAccount.LastEvent)
                            {
                                editRecord.AddField("LastEvent", targetAccount.LastEvent, userAccount.LastEvent);

                                targetAccount.LastEvent = userAccount.LastEvent;
                            }
                        }

                        instance.Context.SaveChanges();

                        instance.SetData(new UserAccountDto(targetAccount), "UserAccount");
                    }
                    else
                    {
                        instance.Status = AppResponseStatus.ResponseError;

                        instance.StatusCode = "ACCESS_DENIED";
                    }
                }
                else
                {
                    instance.Status = AppResponseStatus.ResponseError;

                    instance.StatusCode = "INVALID_ACCOUNT";
                }
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;

                instance.StatusCode = "ACCESS_DENIED";
            }

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("account")]
        public HttpResponseMessage CreateAccount([FromBody] UserAccountDto dto)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            if (instance.Accounts.CheckAccess(instance.LocalAccount, AccountManager.FlagCreateAccount))
            {
                UserAccount account = new UserAccount();

                account.AccountType = dto.AccountType;
                account.Gender = dto.Gender;
                account.FirstName = dto.FirstName;
                account.LastName = dto.LastName;
                account.Birthday = dto.Birthday;
                account.ContactEmail = dto.ContactEmail;
                account.ContactPhone = dto.ContactPhone;
                account.ContactFacebook = dto.ContactFacebook;
                account.ContactSteam = dto.ContactSteam;

                account.TotalEvents = dto.TotalEvents;
                account.EventOffset = dto.EventOffset;
                account.RemoteEvents = dto.RemoteEvents;
                account.LastEvent = dto.LastEvent;
                account.DisplayName = dto.DisplayName;
                account.Visibility = dto.Visibility;

                account.AwardsEnabled = dto.AwardsEnabled;
                account.AwardsXpEnabled = dto.AwardsXpEnabled;

                instance.Accounts.AddAccount(account);

                instance.Context.SaveChanges();

                instance.SetData(new UserAccountDto(account), "UserAccount");
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;

                instance.StatusCode = "ACCESS_DENIED";
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("browse")]
        public HttpResponseMessage BrowseAccounts([FromUri] SearchAccountRequest request)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            request.SanityCheck();

            List<UserAccount> dataAccounts = instance.Accounts.GetAccountSearch(request);

            if (dataAccounts != null)
            {
                BrowseResult<UserAccountDto> accounts = new BrowseResult<UserAccountDto>();

                accounts.TotalResults = instance.Accounts.GetAccountTotal();

                foreach (UserAccount account in dataAccounts)
                {
                    accounts.Results.Add(new UserAccountDto(account));
                }

                instance.SetData(accounts, "UserAccountList");
            }
            else
            {
                instance.Status = AppResponseStatus.AppError;
                instance.StatusCode = "DAO_ERROR";
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("whoami")]
        public HttpResponseMessage WhoAmI()
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            if (instance.LocalAccount != null)
            {
                instance.SetData(new UserAccountDto(instance.LocalAccount), "UserAccount");
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;

                instance.StatusCode = "INVALID_ACCOUNT";
            }

            return instance.ToResponse();
        }

        [HttpPost] 
        [Route("account/{id}/avatar")]
        public HttpResponseMessage SetAvatar(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            ContentManager contentManager = new ContentManager(instance);

            UserAccount account = instance.LocalAccount;

            if (account != null)
            {
                ContentItem content = contentManager.GetItemById(id);

                if (content != null && content.Visible)
                {
                    if (content.IsImage())
                    {
                        account.Avatar = id;

                        NetMessageManager.AddMessageBroadcastQuick(instance, new ChangeAvatarMessage(account));

                        instance.SetData(true, "bool");
                    }
                    else
                    {
                        instance.Status = AppResponseStatus.ResponseError;
                        instance.StatusCode = "INVALID_CONTENT_TYPE";
                    }
                }
                else
                {
                    instance.Status = AppResponseStatus.ResponseError;
                    instance.StatusCode = "INVALID_ID";
                }
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "ACCESS_DENIED";
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("account/{id}/auth/user")]
        public HttpResponseMessage GetAccountUsernames(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;

            // Check if request is valid
            if (id > 0)
            {
                // Check if user has access
                if (localAccount != null && (localAccount.Id == id || instance.Accounts.CheckAccess(localAccount, AccountManager.FlagEditUsername)))
                {
                    UserAccount targetAccount = instance.Accounts.GetAccountReadOnly(id);

                    // Check if target is valid
                    if (targetAccount != null)
                    {
                        // Check if target is protected
                        if (!targetAccount.Root || targetAccount.Id == localAccount.Id)
                        {
                            List<AuthUsername> usernames = instance.Accounts.GetUsernames(targetAccount);
                            List<AuthUsernameDto> usernameModels = new List<AuthUsernameDto>();

                            // Convert usernames to specified username model to protect hashed passwords
                            foreach (AuthUsername username in usernames)
                            {
                                usernameModels.Add(new AuthUsernameDto(username));
                            }

                            instance.SetData(usernameModels, "AuthUsernameList");
                        }
                        else
                        {
                            instance.Status = AppResponseStatus.ResponseError;
                            instance.StatusCode = "ACCESS_DENIED";
                        }
                    }
                    else
                    {
                        instance.Status = AppResponseStatus.ResponseError;
                        instance.StatusCode = "INVALID_ACCOUNT";
                    }
                }
                else
                {
                    instance.Status = AppResponseStatus.ResponseError;
                    instance.StatusCode = "ACCESS_DENIED";
                }
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "INVALID_REQUEST";
            }

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("account/{id}/auth/user")]
        public HttpResponseMessage CreateUsername(long id, [FromBody] AddUsernameRequest request)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;

            // Check if request is valid
            if (id > 0 && request.Username.Length > 0 && request.Password.Length > 0)
            {
                // Check user's access
                if (localAccount != null && instance.Accounts.CheckAccess(localAccount, AccountManager.FlagEditUsername))
                {
                    UserAccount targetAccount = instance.Accounts.GetAccount(id);

                    // Check if target is valid
                    if (targetAccount != null)
                    {
                        // Check if target is protected
                        if (!targetAccount.Root || targetAccount.Id == localAccount.Id)
                        {
                            // Check if username already exists
                            if (instance.Accounts.GetUsername(request.Username) == null)
                            {
                                AuthUsername username = instance.Accounts.CreateUsername(id, request.Username, request.Password);

                                instance.Context.SaveChanges();

                                instance.SetData(new AuthUsernameDto(username), "AuthUsername");
                            }
                            else
                            {
                                instance.Status = AppResponseStatus.ResponseError;
                                instance.StatusCode = "USERNAME_EXISTS";
                            }
                        }
                        else
                        {
                            instance.Status = AppResponseStatus.ResponseError;
                            instance.StatusCode = "ACCESS_DENIED";
                        }
                    }
                    else
                    {
                        instance.Status = AppResponseStatus.ResponseError;
                        instance.StatusCode = "INVALID_ACCOUNT";
                    }
                }
                else
                {
                    instance.Status = AppResponseStatus.ResponseError;
                    instance.StatusCode = "ACCESS_DENIED";
                }
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "INVALID_REQUEST";
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("account/{id}/auth/user/{authId}")]
        public HttpResponseMessage EditUsername(long id, long authId, [FromBody] AuthUsernameDto username)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;

            // Check if request is valid
            if (id > 0 && username.Username.Length > 0 && username.Password.Length > 0)
            {
                // Check user's access
                if (localAccount != null && instance.Accounts.CheckAccess(localAccount, AccountManager.FlagEditUsername))
                {
                    UserAccount targetAccount = instance.Accounts.GetAccount(id);
                }

            }

            return instance.ToResponse();
        }

        [HttpDelete]
        [Route("account/{id}/auth/user/{authId}")]
        public HttpResponseMessage DeleteUsername(long id, long authId)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            // TODO: This

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("account/{id}/auth/session")]
        public HttpResponseMessage GetAccountSessions(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;

            // Check if request is valid
            if (id > 0)
            {
                // Check if user has access
                if (localAccount != null && (localAccount.Id == id || instance.Accounts.CheckAccess(localAccount, AccountManager.FlagEditSession)))
                {
                    UserAccount targetAccount = instance.Accounts.GetAccountReadOnly(id);

                    // Check if target is valid
                    if (targetAccount != null)
                    {
                        // Check if target is protected
                        if (!targetAccount.Root || targetAccount.Id == localAccount.Id)
                        {
                            List<AuthSession> sessions = instance.Accounts.GetSessions(targetAccount);
                            List<AuthSessionDto> sessionModels = new List<AuthSessionDto>();

                            foreach (AuthSession session in sessions)
                            {
                                sessionModels.Add(new AuthSessionDto(session));
                            }

                            instance.SetData(sessionModels, "AuthSessionList");
                        }
                        else
                        {
                            instance.Status = AppResponseStatus.ResponseError;
                            instance.StatusCode = "ACCESS_DENIED";
                        }
                    }
                    else
                    {
                        instance.Status = AppResponseStatus.ResponseError;
                        instance.StatusCode = "INVALID_ACCOUNT";
                    }
                }
                else
                {
                    instance.Status = AppResponseStatus.ResponseError;
                    instance.StatusCode = "ACCESS_DENIED";
                }
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "INVALID_REQUEST";
            }

            return instance.ToResponse();
        }

        [HttpDelete]
        [Route("account/{id}/auth/session/{authId}")]
        public HttpResponseMessage DeleteSession(long id, long authId)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            // TODO: This

            return instance.ToResponse();
        }

        // Authentication

        [HttpPost]
        [Route("logout")]
        public HttpResponseMessage Logout()
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            
            if (instance.LocalAccount != null)
            {
                HttpCookie sessionId = HttpContext.Current.Request.Cookies["LPSessionId"];

                instance.Accounts.RemoveSession(AuthSession.GetIdFromCookie(sessionId));

                CookieHeaderValue sessionIdCookie = new CookieHeaderValue("LPSessionId", "");
                CookieHeaderValue sessionKeyCookie = new CookieHeaderValue("LPSessionKey", "");

                sessionIdCookie.Expires = DateTimeOffset.Now.AddDays(-1d);
                sessionKeyCookie.Expires = DateTimeOffset.Now.AddDays(-1d);

                instance.Cookies.Add(sessionIdCookie);
                instance.Cookies.Add(sessionKeyCookie);
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("login/user")]
        public HttpResponseMessage LoginUsername([FromBody] UserLoginRequest request)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.Accounts.AuthByUsername(request.Username, request.Password);

            if (localAccount != null)
            {
                AuthSession session = instance.Accounts.CreateSession(localAccount);

                instance.Context.SaveChanges();

                DateTimeOffset expiration = DateTimeOffset.Now.AddDays(30);

                instance.AddCookie("LPSessionId", session.Id.ToString(), expiration);
                instance.AddCookie("LPSessionKey", session.Key, expiration);

                NetMessageManager.AddMessageBroadcastQuick(instance, new NewActiveUserMessage(localAccount.Id));
                instance.Accounts.UpdateActivity(localAccount);

                // TODO: Log successful attempt

                instance.SetData(new UserAccountDto(localAccount), "UserAccount");
            }
            else
            {
                // TODO: Log failed attempt

                instance.Status = AppResponseStatus.ResponseError;

                instance.StatusCode = "INVALID_ACCOUNT";
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("local/access/{scope}/{flag}")]
        public HttpResponseMessage CheckLocalAccess(String flag, String scope)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;

            if (localAccount != null)
            {
                instance.SetData(instance.Accounts.CheckAccess(localAccount, flag, scope, false), "bool");
            }

            return instance.ToResponse();
        }

        // Roles/Access

        [HttpGet]
        [Route("role/{id}")]
        public HttpResponseMessage GetRole(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserRole role = instance.Accounts.GetRoleById(id);

            if (role != null)
            {
                instance.SetData(new UserRoleDto(role), "UserRole");
            }
            else
            {
                instance.SetError("INVALID_ROLE");
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("role/{id}")]
        public HttpResponseMessage EditRole(long id, [FromBody] UserRoleDto roleEdit)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            if (instance.Accounts.CheckAccess(AccountManager.FlagEditRoles))
            {
                UserRole role = instance.Accounts.GetRoleById(id);

                if (role != null)
                {
                    role.Name = roleEdit.Name;

                    instance.Context.SaveChanges();

                    instance.SetData(new UserRoleDto(role), "UserRole");
                }
                else
                {
                    instance.SetError("INVALID_ROLE");
                }
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("role")]
        public HttpResponseMessage CreateRole([FromBody] UserRoleDto role)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            if (instance.Accounts.CheckAccess(AccountManager.FlagEditRoles))
            {
                UserRole newRole = new UserRole();

                newRole.Name = role.Name;

                instance.Accounts.AddRole(newRole);

                instance.Context.SaveChanges();

                instance.SetData(new UserRoleDto(newRole), "UserRole");

                // TODO: Log
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("role/{id}/permission")]
        public HttpResponseMessage GetRolePermissions(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            if (instance.LocalAccount != null)
            {
                List<UserPermission> permissions = instance.Accounts.GetRolePermissions(id);

                instance.SetData(UserPermissionDto.ConvertList(permissions), "UserPermissionList");
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("role/{id}/permission")]
        public HttpResponseMessage AddRolePermission(long id, [FromBody] UserPermissionDto permission)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            if (instance.Accounts.CheckAccess(AccountManager.FlagEditRoles))
            {
                if (permission.Flag.Length > 0 && permission.Scope.Length > 0)
                {
                    UserRole role = instance.Accounts.GetRoleById(id);

                    if (role != null)
                    {
                        UserPermission newPermission = instance.Accounts.GetPermission(id, permission.Flag,
                            permission.Scope);

                        if (newPermission == null)
                        {
                            newPermission = new UserPermission();

                            newPermission.Role = id;
                            newPermission.Flag = permission.Flag;
                            newPermission.Scope = permission.Scope;

                            instance.Accounts.AddPermission(newPermission);

                            instance.Context.SaveChanges();
                        }

                        instance.SetData(new UserPermissionDto(newPermission), "UserPermission");
                    }
                    else
                    {
                        instance.SetError("INVALID_ROLE");
                    }
                }
                else
                {
                    instance.SetError("INVALID_PERMISSION");
                }
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpDelete]
        [Route("role/{id}/permission")]
        public HttpResponseMessage DeleteRolePermission(long id, [FromBody] UserPermissionDto permission)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);

            if (instance.Accounts.CheckAccess(AccountManager.FlagEditRoles))
            {
                UserPermission target = instance.Accounts.GetPermission(id, permission.Flag, permission.Scope);

                if (target != null)
                {
                    instance.Accounts.RemovePermission(target);

                    instance.Data = true;
                }
                else
                {
                    instance.SetError("INVALID_PERMISSION");
                }
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }
    }
}
