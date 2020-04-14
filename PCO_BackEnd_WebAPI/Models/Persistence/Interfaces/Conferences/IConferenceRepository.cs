using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.DTOs.Conferences;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences
{
    public interface IConferenceRepository : IRepository<Conference>
    {
        PageResult<Conference> GetPagedConferences(int page, int size, string filter = null, string day = null, string month = null,
                                                         string year = null, string fromDate = null, string toDate = null);
        Conference GetConferenceByTitle(string title);
        Conference UpdateConferenceInfo(int id, Conference conference, string base64Image);
    }
}
