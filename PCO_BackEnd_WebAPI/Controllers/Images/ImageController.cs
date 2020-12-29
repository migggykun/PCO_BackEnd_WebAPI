using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.Images
{
    /// <summary>
    /// Controller for Images
    /// </summary>
    public class ImageController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default Constructor. Initialize Database.
        /// </summary>
        public ImageController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets banner or receipt
        /// </summary>
        /// <param name="identifier">1: banner , 2: receipts</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetImages(int identifier)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            IEnumerable<PcoImage> images = null;

            if (identifier == ImageIdentifier.BANNER)
            {
                images = await Task.Run(()=>unitOfWork.Banners.GetAll().OfType<PcoImage>());
            }
            else if (identifier == ImageIdentifier.RECEIPT)
            {
                images = await Task.Run(()=>unitOfWork.Receipts.GetAll().OfType<PcoImage>());
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
        /// Get Single Image based on ID
        /// </summary>
        /// <param name="id">image id</param>
        /// <param name="identifier">1: banner , 2: receipts</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetSingleImage")]
        public async Task<IHttpActionResult> GetImages(int id, int identifier)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            PcoImage image = null;

            if (identifier == ImageIdentifier.BANNER)
            {
                image = await Task.Run(()=>unitOfWork.Banners.Get(id) as PcoImage);
            }
            else if (identifier == ImageIdentifier.RECEIPT)
            {
                image = await Task.Run(()=>unitOfWork.Receipts.Get(id) as PcoImage);
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
