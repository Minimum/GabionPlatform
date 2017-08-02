using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GabionPlatform.Controllers
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        [HttpGet]
        [Route("info/id/{id}")]
        public HttpResponseMessage GetGameById(long id)
        {
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);



            return response;
        }

    }
}
