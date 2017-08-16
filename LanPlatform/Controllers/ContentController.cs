using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GabionPlatform.Content;
using GabionPlatform.DTO.Content;
using GabionPlatform.Engine;
using GabionPlatform.Models;
using Newtonsoft.Json;

namespace GabionPlatform.Controllers
{
    [RoutePrefix("api/content")]
    public class ContentController : ApiController
    {
        [HttpGet]
        [Route("view/id/{id}")]
        public HttpResponseMessage GetContentInfoById(long id)
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            ContentManager contentManager = new ContentManager(instance);

            if (id > 0)
            {
                ContentItem item = contentManager.GetItemById(id);

                if (item != null)
                {
                    if (contentManager.CheckAccess(item, instance.LocalAccount))
                    {
                        instance.Data = new ContentItemDto(item);
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
                    instance.StatusCode = "CONTENT_DOES_NOT_EXIST";
                }
            }

            return instance.ToResponse();
        }

        [HttpGet]
        [Route("data/id/{id}")]
        public HttpResponseMessage GetContentDataById(long id)
        {
            HttpResponseMessage response = null;
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            ContentManager contentManager = new ContentManager(instance);

            if (id > 0)
            {
                ContentItem item = contentManager.GetItemById(id);

                if (item != null)
                {
                    if (contentManager.CheckAccess(item, instance.LocalAccount))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);

                        response.Content = new StreamContent(contentManager.GetDataStream(item));

                        response.Content.Headers.ContentType = new MediaTypeHeaderValue(item.DataMime);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }

            return response;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> UploadContent()
        {
            AppInstance instance = new AppInstance(Request, HttpContext.Current);
            ContentManager contentManager = new ContentManager(instance);

            if (instance.LocalAccount != null)
            {
                if (Request.Content.IsMimeMultipartContent())
                {
                    MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();

                    await Request.Content.ReadAsMultipartAsync(provider);

                    ContentItem item = new ContentItem();

                    foreach (HttpContent file in provider.Contents)
                    {
                        byte[] data = await file.ReadAsByteArrayAsync();
                        
                        item.Owner = instance.LocalAccount.Id;
                        item.Hash = ContentManager.GetDataHash(data);
                        item.Filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                        item.Size = data.LongLength;
                        item.Type = ContentManager.GetContentType(MimeMapping.GetMimeMapping(item.Filename));
                        item.TimeAdded = EngineUtil.CurrentTime;

                        contentManager.AddItem(item);
                        contentManager.SaveData(item, data);

                        break;
                    }

                    return Ok(JsonConvert.SerializeObject(item));
                }

                return Conflict();
            }

            return Unauthorized();
        }
    }
}