using System;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Web;
using System.Web.Http;
using GabionPlatform.Accounts;
using GabionPlatform.Content;
using GabionPlatform.Database;
using GabionPlatform.Settings;

namespace GabionPlatform
{
    public class GabionPlatform : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            
        }

        protected void Application_BeginRequest()
        {

        }

        protected void Application_EndRequest()
        {

        }
        
    }
}
