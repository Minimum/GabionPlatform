using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using GabionPlatform.Events;
using GabionPlatform.Models;
using Newtonsoft.Json;

namespace GabionPlatform.Controllers
{
    [RoutePrefix("api/event")]
    public class EventController : ApiController
    {
        [HttpGet]
        [Route("info/{id}")]
        public HttpResponseMessage GetInfo(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            LanEventManager events = new LanEventManager(instance);

            instance.Data = events.GetEventById(id);

            if (instance.Data == null)
            {
                instance.Status = AppResponseStatus.ResponseError;
                instance.StatusCode = "INVALID_EVENT";
            }

            return instance.ToResponse();
        }
    }
}
