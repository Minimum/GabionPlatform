using System.Net.Http;
using System.Web;
using System.Web.Http;
using GabionPlatform.Accounts;
using GabionPlatform.Apps;
using GabionPlatform.DTO.Apps;
using GabionPlatform.Models;
using GabionPlatform.Models.Requests;
using GabionPlatform.Network;
using GabionPlatform.Network.Messages;

namespace GabionPlatform.Controllers
{
    [RoutePrefix("api/apps")]
    public class AppController : ApiController
    {
        [HttpGet]
        [Route("app/{id}")]
        public HttpResponseMessage GetApp(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            App app = apps.GetAppById(id);

            if (app != null)
            {
                instance.SetData(new AppDto(app), "App");
            }
            else
            {
                instance.SetError("INVALID_APP");
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("app/{id}")]
        public HttpResponseMessage EditApp(long id, [FromBody] AppDto edit)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            if (instance.Accounts.CheckAccess(AppManager.FlagAppEdit))
            {
                App app = apps.GetAppById(id);

                if (app != null)
                {
                    // TODO: Add edit logging

                    app.Type = edit.Type;
                    app.Title = edit.Title;
                    app.Description = edit.Description;
                    app.DownloadType = edit.DownloadType;
                    app.DownloadInfo = edit.DownloadInfo;

                    instance.SetData(new AppDto(app), "App");
                }
                else
                {
                    instance.SetError("INVALID_APP");
                }
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpDelete]
        [Route("app/{id}")]
        public HttpResponseMessage DeleteApp(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            if (instance.Accounts.CheckAccess(AppManager.FlagAppEdit))
            {
                App app = apps.GetAppById(id);

                if (app != null)
                {
                    instance.SetData(new AppDto(app), "app");

                    apps.RemoveApp(app);
                }
                else
                {
                    instance.SetError("INVALID_APP");
                }
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("app")]
        public HttpResponseMessage CreateApp(long id, [FromBody] AppDto app)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            if (instance.Accounts.CheckAccess(AppManager.FlagAppEdit))
            {
                App newApp = new App();

                newApp.Type = app.Type;
                newApp.Title = app.Title;
                newApp.Description = app.Description;
                newApp.DownloadType = app.DownloadType;
                newApp.DownloadInfo = app.DownloadInfo;

                apps.AddApp(newApp);

                instance.Context.SaveChanges();

                instance.SetData(new AppDto(newApp), "App");
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("loaners")]
        public HttpResponseMessage GetAllLoaners()
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            instance.Data = LoanerAccountDto.ConvertList(apps.GetLoanerAccounts());

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("loaner/{id}")]
        public HttpResponseMessage GetLoaner(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            instance.Data = new LoanerAccountDto(apps.GetLoanerAccountById(id));

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("loaner")]
        public HttpResponseMessage CreateLoaner([FromBody] LoanerAccountDto account)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (instance.Accounts.CheckAccess(localAccount, AppManager.FlagLoanerEdit))
            {
                if (account.Username.Length > 0)
                {
                    LoanerAccount loaner = new LoanerAccount();

                    loaner.Username = account.Username;
                    loaner.Password = account.Password;

                    apps.AddLoanerAccount(loaner);

                    instance.Context.SaveChanges();

                    instance.SetData(loaner, "LoanerAccount");
                }
                else
                {
                    instance.SetError("INVALID_ACCOUNT");
                }
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("loaner/{id}/app/{appId}")]
        public HttpResponseMessage CreateLoanerApp(long id, long appId)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            if (instance.Accounts.CheckAccess(AppManager.FlagLoanerEdit))
            {
                LoanerAccount account = apps.GetLoanerAccountById(id);

                if (account != null)
                {
                    LoanerApp loanerApp = apps.GetLoanerApp(account, appId);

                    if (loanerApp == null)
                    {
                        App app = apps.GetAppById(appId);

                        if (app != null)
                        {
                            loanerApp = new LoanerApp();

                            loanerApp.Account = id;
                            loanerApp.App = appId;

                            apps.AddLoanerApp(loanerApp);

                            instance.Context.SaveChanges();

                            instance.SetData(loanerApp, "LoanerApp");
                        }
                        else
                        {
                            instance.SetError("INVALID_APP");
                        }
                    }
                    else
                    {
                        instance.SetData(loanerApp, "LoanerApp");
                    }
                }
                else
                {
                    instance.SetError("INVALID_ACCOUNT");
                }
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpDelete]
        [Route("loaner/{id}/app/{appId}")]
        public HttpResponseMessage DeleteLoanerApp(long id, long appId)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            if (instance.Accounts.CheckAccess(AppManager.FlagLoanerEdit))
            {
                LoanerAccount loaner = apps.GetLoanerAccountById(id);

                if (loaner != null)
                {
                    LoanerApp app = apps.GetLoanerApp(loaner, appId);

                    if (app != null)
                    {
                        instance.SetData(app, "LoanerApp");

                        apps.RemoveLoanerApp(app);
                    }
                    else
                    {
                        instance.SetError("INVALID_APP");
                    }
                }
                else
                {
                    instance.SetError("INVALID_ACCOUNT");
                }
            }
            else
            {
                instance.SetError("ACCESS_DENIED");
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("loaner/{id}/checkout")]
        public HttpResponseMessage CheckoutLoaner(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (localAccount == null)
            {
                instance.SetError("ACCESS_DENIED");
            }
            else if (apps.GetUserCheckoutCount(localAccount) > 0)
            {
                instance.SetError("CHECKOUT_LIMIT_HIT");
            }
            else if (id > 0)
            {
                LoanerAccount loaner = apps.GetLoanerAccountById(id);

                if (loaner == null)
                {
                    instance.SetError("INVALID_ACCOUNT");
                }
                else if (loaner.CheckoutUser != 0)
                {
                    instance.SetError("ACCOUNT_IN_USE");
                }
                else
                {
                    // Change checkout value
                    loaner.CheckoutUser = localAccount.Id;
                    loaner.CheckoutChallenge++;

                    // Add message
                    NetMessageManager.AddMessageQuick(instance, new LoanerCheckoutMessage(loaner));

                    instance.SetData(true, "bool");
                }
            }
            else
            {
                instance.SetError("INVALID_ACCOUNT");
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("loaner/{id}/checkin")]
        public HttpResponseMessage CheckinLoaner(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (localAccount == null)
            {
                instance.SetError("ACCESS_DENIED");
            }
            else if (id > 0)
            {
                LoanerAccount loaner = apps.GetLoanerAccountById(id);

                if (loaner == null)
                {
                    instance.SetError("INVALID_ACCOUNT");
                }
                else if (loaner.CheckoutUser != localAccount.Id && !instance.Accounts.CheckAccess(localAccount, AppManager.FlagLoanerCheckout))
                {
                    instance.SetError("ACCESS_DENIED");
                }
                else
                {
                    // Change checkout value
                    loaner.CheckoutUser = 0;

                    // Add message
                    NetMessageManager.AddMessageQuick(instance, new LoanerCheckoutMessage(loaner));

                    instance.SetData(true, "bool");
                }
            }
            else
            {
                instance.SetError("INVALID_ACCOUNT");
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("loaner/{id}/steamcode")]
        public HttpResponseMessage SetSteamCode(long id, [FromBody] SetSteamCodeRequest request)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (localAccount != null && instance.Accounts.CheckAccess(localAccount, AppManager.FlagLoanerSteamCode))
            {
                LoanerAccount account = apps.GetLoanerAccountById(id);

                if (account != null)
                {
                    if (account.CheckoutChallenge == request.Challenge && account.CheckoutUser != 0)
                    {
                        account.SteamCode = request.Code;

                        NetMessageManager.AddMessageSingleQuick(instance, account.CheckoutUser, new NewSteamCodeMessage(request.Code));
                    }
                    else
                    {
                        instance.Status = AppResponseStatus.ResponseError;
                        instance.StatusCode = "INVALID_CHALLENGE";
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
                instance.Status = AppResponseStatus.AccessDenied;
                instance.StatusCode = AppManager.FlagLoanerSteamCode;
            }

            return instance.ToResponse();
        }
    }
}
