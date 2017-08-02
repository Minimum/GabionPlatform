using System.Net.Http;
using System.Web.Http;

namespace GabionPlatform.Controllers
{
    [RoutePrefix("api/music")]
    public class MusicController : ApiController
    {
        [HttpGet]
        [Route("song/id/{id}")]
        public HttpResponseMessage GetSongById(long id)
        {
            return null;
        }
    }
}
