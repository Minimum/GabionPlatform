﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using GabionPlatform.Accounts;
using GabionPlatform.DAL;
using GabionPlatform.Settings;
using Newtonsoft.Json;

namespace GabionPlatform.Models
{
    public class AppInstance
    {
        public String AppName
        {
            get { return Properties.Resources.AppName; }
        }

        public String AppBuild
        {
            get { return Properties.Resources.AppBuild; }
        }

        // Context Data
        [JsonIgnore]
        public AccountManager Accounts
        {
            get { return AccountManager; }
        }

        protected AccountManager AccountManager;

        [JsonIgnore]
        public UserAccount LocalAccount
        {
            get { return LocalUserAccount; }
        }

        protected UserAccount LocalUserAccount;

        [JsonIgnore]
        public SettingsManager Settings
        {
            get { return SettingsManager; }
        }

        protected SettingsManager SettingsManager;

        [JsonIgnore]
        public HttpRequestMessage RequestMessage
        {
            get { return Request; }
        }

        protected HttpRequestMessage Request;

        [JsonIgnore]
        public HttpContext RequestContext
        {
            get { return InternalRequestContext; }
        }

        protected HttpContext InternalRequestContext;

        [JsonIgnore]
        public PlatformContext Context
        {
            get { return DataContext; }
        }

        protected PlatformContext DataContext;

        [JsonIgnore]
        public List<CookieHeaderValue> Cookies
        {
            get { return CookieList; }
        }

        protected List<CookieHeaderValue> CookieList;

        // Response Data
        public AppResponseStatus Status { get; set; }
        public String StatusCode { get; set; }

        public String DataType { get; set; }
        public Object Data { get; set; }

        [JsonIgnore]
        public EventHandler OnSaveFailure { get; set; }

        public AppInstance(HttpRequestMessage request, HttpContext requestContext)
        {
            Request = request;
            InternalRequestContext = requestContext;

            Status = AppResponseStatus.ResponseHandled;
            StatusCode = "";

            DataType = "null";
            Data = null;

            OnSaveFailure = delegate { };

            DataContext = new PlatformContext();

            CookieList = new List<CookieHeaderValue>();

            AccountManager = new AccountManager(this);
            SettingsManager = new SettingsManager(this);

            LocalUserAccount = AccountManager.AuthenticateLocalUser();
        }

        public void SetData(Object data, String type)
        {
            Data = data;
            DataType = type;

            return;
        }

        public void AddCookie(String name, String value)
        {
            AddCookie(name, value, DateTimeOffset.Now.AddDays(7));

            return;
        }

        public void AddCookie(String name, String value, DateTimeOffset expiration)
        {
            CookieHeaderValue cookie = new CookieHeaderValue(name, value);
            cookie.Expires = expiration;
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";

            Cookies.Add(cookie);

            return;
        }

        public HttpResponseMessage ToResponse()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Status = AppResponseStatus.AppError;
                
                StatusCode = "CONCURRENCY_ERROR";

                OnSaveFailure.Invoke(this, EventArgs.Empty);
            }

            response.Headers.AddCookies(Cookies);

            response.Content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");

            return response;
        }
    }

    public enum AppResponseStatus
    {
        ResponseHandled = 0,    // Regular response, no errors
        ResponseError,          // Response halted due to a non-system issue
        AppNotInstalled,        // App has not been initialized yet
        AppDisabled,            // App is currently offline
        AppError,               // Response halted due to a system error (IE. database offline)
        AccessDenied            // Access is denied
    }
}