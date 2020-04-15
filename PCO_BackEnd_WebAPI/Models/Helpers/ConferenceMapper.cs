using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Pagination;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class ConferenceMapper
    {
        public static PageResult<ResponseConferenceDTO> MapToPagedResponseConferenceDTO(PageResult<Conference> conferences)
        {
            var resultDTO = PaginationMapper<Conference, ResponseConferenceDTO>.MapResult(conferences);
            foreach (var r in conferences.Results)
            {
                int index = resultDTO.Results.ToList().FindIndex(x => x.Id == r.Id);
                resultDTO.Results[index].Banner = r.Banner == null ? string.Empty : Convert.ToBase64String(r.Banner);
            }
            return resultDTO;
        }

        public static ResponseConferenceDTO MapToResponseConferenceDTO(Conference conference)
        {
            var resultDTO = Mapper.Map<Conference, ResponseConferenceDTO>(conference);
            resultDTO.Banner = conference.Banner == null ? string.Empty : Convert.ToBase64String(conference.Banner);
            return resultDTO;
        }
    }
}