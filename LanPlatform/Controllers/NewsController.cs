using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using GabionPlatform.Accounts;
using GabionPlatform.Models;
using GabionPlatform.Models.Requests;
using GabionPlatform.Network;
using GabionPlatform.Network.Messages;
using GabionPlatform.News;
using GabionPlatform.Settings;
using Newtonsoft.Json;
using GabionPlatform.Models.Responses;

namespace GabionPlatform.Controllers
{
    [RoutePrefix("api/news")]
    public class NewsController : ApiController
    {
        [HttpGet]
        [Route("current")]
        public HttpResponseMessage GetCurrentStatus()
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            NewsManager newsManager = new NewsManager(instance);

            instance.Data = newsManager.GetCurrentStatus();

            if (instance.Data == null)
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "NEWS_NOT_FOUND";
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("current")]
        public HttpResponseMessage ChangeStatus([FromBody] ChangeNewsStatusRequest request)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;
            NewsManager newsManager = new NewsManager(instance);

            if (localAccount != null)
            {
                // TODO: 
                if (instance.Accounts.CheckAccess(localAccount, "NewsChangeStatus", "platform"))
                {
                    if (newsManager.GetStatusById(request.Id) != null)
                    {
                        instance.Settings.ChangeSetting("NewsStatus", request.Id.ToString());

                        NetMessageManager.AddMessageBroadcastQuick(instance, new NewsStatusChangeMessage(request.Id));
                    }
                    else
                    {
                        instance.Status = AppResponseStatus.ResponseError;

                        instance.StatusCode = "ACCESS_STATUS";
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

                instance.StatusCode = "ACCESS_DENIED";
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("status/{id}")]
        public HttpResponseMessage GetStatusById(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            NewsManager newsManager = new NewsManager(instance);
            NewsStatus status = newsManager.GetStatusById(id);

            if (status != null)
            {
                instance.Data = new NewsStatusModel(status);
            }
            else
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "INVALID_STATUS";
            }

            return instance.ToResponse();
        }

        [HttpPost]
        [Route("status/{id}")]
        public HttpResponseMessage EditStatusById(long id, [FromBody] EditNewsStatusRequest request)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;
            NewsManager newsManager = new NewsManager(instance);

            if (localAccount != null)
            {
                if (instance.Accounts.CheckAccess(localAccount, "NewsEditStatus"))
                {
                    NewsStatus status = newsManager.GetStatusById(id);

                    if (status != null)
                    {
                        status.Title = request.Title;
                        status.Content = request.Content;

                        PlatformSetting statusSetting = instance.Settings.GetSettingByName("NewsStatus");

                        // TODO: Make a proper message for this
                        if (statusSetting != null && statusSetting.ToInt64() == id)
                        {
                            NetMessageManager.AddMessageBroadcastQuick(instance, new NewsStatusChangeMessage(id));
                        }

                        instance.Data = status;
                    }
                    else
                    {
                        instance.Status = AppResponseStatus.ResponseError;
                        instance.StatusCode = "INVALID_STATUS";
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
                instance.StatusCode = "ACCESS_DENIED";
            }

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("status")]
        public HttpResponseMessage CreateStatus([FromBody] EditNewsStatusRequest request)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            UserAccount localAccount = instance.LocalAccount;
            NewsManager newsManager = new NewsManager(instance);

            if (localAccount != null)
            {
                if (instance.Accounts.CheckAccess(localAccount, "NewsEditStatus"))
                {
                    NewsStatus status = new NewsStatus();

                    status.Title = request.Title;
                    status.Content = request.Content;

                    // Add and save status to DB
                    newsManager.AddStatus(status);

                    instance.Data = status;
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
                instance.StatusCode = "ACCESS_DENIED";
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("browse/status/{page}")]
        public HttpResponseMessage BrowseStatus(int page)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            NewsManager newsManager = new NewsManager(instance);

            page = page < 1 ? 1 : page;

            instance.Data = new BrowseResult<NewsStatus>(newsManager.GetStatusList(page, 50), newsManager.GetStatusCount());

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("weather")]
        public HttpResponseMessage GetWeather()
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            NewsManager newsManager = new NewsManager(instance);

            instance.Data = newsManager.GetCurrentWeatherStatus();

            if (instance.Data == null)
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "STATUS_NOT_FOUND";
            }

            return instance.ToResponse();
        }

        [HttpPut]
        [Route("weather")]
        public HttpResponseMessage SetWeather([FromBody] WeatherStatus status)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            NewsManager newsManager = new NewsManager(instance);
            UserAccount localAccount = instance.LocalAccount;

            if (status != null)
            {
                if (localAccount != null && instance.Accounts.CheckAccess(localAccount, NewsManager.FlagChangeWeather))
                {
                    newsManager.AddWeatherStatus(status);
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
    }
}
