using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Images.Helpers;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PCO_BackEnd_WebAPI.Roles;
using PCO_BackEnd_WebAPI.Security.OAuth;

namespace PCO_BackEnd_WebAPI.Controllers.Images
{
    public class ImageController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public ImageController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets banner or receipt
        /// </summary>
        /// <param name="identifier">1: banner , 2: receipts</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = UserRoles.ROLE_ADMIN)]
        [HttpGet]
        public async Task<IHttpActionResult> GetImages(int identifier)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            IEnumerable<PcoImage> images = null;

            if (identifier == ImageIdentifier.BANNER)
            {
                images = unitOfWork.Banners.GetAll().OfType<PcoImage>();
            }
            else if (identifier == ImageIdentifier.RECEIPT)
            {
                images = unitOfWork.Receipts.GetAll().OfType<PcoImage>();
            }
            else
            {
                return BadRequest("Invalid Identifier");
            }

            if (images == null)
            {
                return BadRequest();
            }
            else
            {
                var resultDTO = ImageMappers.MaptoListImageDTO(images);
                return Ok(resultDTO);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">image id</param>
        /// <param name="identifier">1: banner , 2: receipts</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/GetSingleImage")]
        public async Task<IHttpActionResult> GetImages(int id, int identifier)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            PcoImage image = null;

            if (identifier == ImageIdentifier.BANNER)
            {
                image = unitOfWork.Banners.Get(id) as PcoImage;
            }
            else if (identifier == ImageIdentifier.RECEIPT)
            {
                image = unitOfWork.Receipts.Get(id) as PcoImage;
            }
            else
            {
                return BadRequest("Invalid Identifier");
            }
            
            if (image == null)
            {
                return NotFound();
            }
            else
            {
                var resultDTO = image.MapToImageDTO();
                return Ok(resultDTO);
            }
        }


    }
}