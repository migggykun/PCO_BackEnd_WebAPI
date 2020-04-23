using PCO_BackEnd_WebAPI.DTOs.Images;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Images.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class ImageMappers
    {
        public static IEnumerable<ResponseImageDTO> MaptoListImageDTO(IEnumerable<PcoImage> images)
        {
            List<ResponseImageDTO> imagesDTO = new List<ResponseImageDTO>();

            foreach (var i in images)
            {
                imagesDTO.Add
                    (
                        i.MapToImageDTO()
                    );
            }

            return imagesDTO;
        }

        public static ResponseImageDTO MapToImageDTO(this PcoImage image)
        {
            return new ResponseImageDTO
                    {
                        Id = image.Id,
                        Image = ImageFormatter.GetImageStringFormat(image.Image)
                    };
        }
    }
}