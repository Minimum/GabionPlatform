using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using GabionPlatform.Accounts;
using GabionPlatform.Apps;
using GabionPlatform.Models;
using GabionPlatform.Models.Requests;
using GabionPlatform.Network;
using GabionPlatform.Network.Messages;
using Newtonsoft.Json;

namespace GabionPlatform.Controllers
{
    [RoutePrefix("api/loaners")]
    public class LoanerController : ApiController
    {
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage GetAll()
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            instance.Data = apps.GetLoanerAccounts();

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("account/{id}")]
        public HttpResponseMessage GetAccount(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);

            instance.Data = apps.GetLoanerAccountById(id);

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("account")]
        public HttpResponseMessage CreateAccount([FromBody] LoanerAccount account)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (instance.Accounts.CheckAccess(localAccount, AppManager.FlagLoanerEdit))
            {
                
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "ACCESS_DENIED";
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("account/{id}/checkout")]
        public HttpResponseMessage Checkout(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (id > 0 && localAccount != null && apps.GetUserCheckoutCount(localAccount) == 0)
            {
                LoanerAccount loaner = apps.GetLoanerAccountById(id);

                if (loaner != null && (loaner.CheckoutUser == 0 || instance.Accounts.CheckAccess(localAccount, AppManager.FlagLoanerCheckout)))
                {
                    // Change checkout value
                    loaner.CheckoutUser = localAccount.Id;
                    loaner.CheckoutChallenge++;

                    // Add message
                    NetMessageManager.AddMessageQuick(instance, new LoanerCheckoutMessage(loaner));

                    try
                    {
                        // Save before we launch SteamCodeHelper
                        instance.Context.SaveChanges();

                        // Launch SteamCodeHelper
                        Process.Start(ConfigurationManager.AppSettings["LPSteamCodeHelperLocation"], loaner.Username + " " + loaner.CheckoutChallenge);
                        
                        instance.Data = true;
                    }
                    catch (Exception e)
                    {
                        instance.Status = AppResponseStatus.ResponseError;
                        instance.StatusCode = "UNHANDLED EXCEPTION: " + e.Message;
                    }
                }
                else
                {
                    instance.Data = false;

                    // TODO: Log failed attempt
                }
            }
            else
            {
                instance.Data = false;
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("account/{id}/checkin")]
        public HttpResponseMessage Checkin(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (id > 0 && localAccount != null)
            {
                LoanerAccount loaner = apps.GetLoanerAccountById(id);

                if (loaner != null && (loaner.CheckoutUser == localAccount.Id || instance.Accounts.CheckAccess(localAccount, AppManager.FlagLoanerCheckout)))
                {
                    // Change checkout value
                    loaner.CheckoutUser = 0;

                    // Add message
                    NetMessageManager.AddMessageQuick(instance, new LoanerCheckoutMessage(loaner));

                    instance.Data = true;
                }
                else
                {
                    instance.Data = false;
                }
            }
            else
            {
                instance.Data = false;
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("steamcode")]
        public HttpResponseMessage SetSteamCode([FromBody] SetSteamCodeRequest request)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            AppManager apps = new AppManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (localAccount != null && instance.Accounts.CheckAccess(localAccount, AppManager.FlagLoanerSteamCode))
            {
                LoanerAccount account = apps.GetLoanerAccountByName(request.Username);

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
