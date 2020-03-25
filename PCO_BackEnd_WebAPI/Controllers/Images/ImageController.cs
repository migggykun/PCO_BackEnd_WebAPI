using PCO_BackEnd_WebAPI.DTOs.Pictures;
using PCO_BackEnd_WebAPI.Models.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.Images
{
    [RoutePrefix("api/Image")]
    public class ImageController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> UploadImage(RequestPictureDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ImageManager manager = new ImageManager();
            string link = await Task.Run(() => manager.UploadImage(model.image));
            return Ok(link);
        }
    

    }
}
