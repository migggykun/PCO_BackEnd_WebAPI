using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Conferences;
using System.Data.Entity;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using AutoMapper;
using System.Globalization;
using PCO_BackEnd_WebAPI.Models.Images;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
        public ConferenceRepository(DbContext context) : base(context)
        {
              
        }

        public PageResult<Conference> GetPagedConferences(int page, int size, string filter = null, string day = null, string month = null, 
                                                         string year = null, string fromDate = null, string toDate = null)
        {
            DateTime? startDate = string.IsNullOrEmpty(fromDate) ? null : Convert.ToDateTime(fromDate, new CultureInfo("fil-PH")) as DateTime?;
            DateTime? endDate = string.IsNullOrEmpty(toDate) ? null : Convert.ToDateTime(toDate, new CultureInfo("fil-PH")) as DateTime?;
            IQueryable<Conference> queryResult = appDbContext.Conferences.Where(c => string.IsNullOrEmpty(filter) ? true : c.Title.Contains(filter))
                                                         .Where(c => string.IsNullOrEmpty(day) ? true : c.Start.Day.ToString().Contains(day)
                                                                || c.End.Day.ToString().Contains(day))
                                                         .Where(c => string.IsNullOrEmpty(month) ? true : c.Start.Month.ToString().Contains(month)
                                                                || c.End.Month.ToString().Contains(month))
                                                         .Where(c => string.IsNullOrEmpty(year) ? true : c.Start.Year.ToString().Contains(year)
                                                                || c.End.Year.ToString().Contains(year))
                                                         .Where(c => string.IsNullOrEmpty(fromDate) ? true : c.Start >= startDate)
                                                         .Where(c => string.IsNullOrEmpty(toDate) ? true : c.End <= endDate);

            PageResult<Conference> pageResult = new PageResult<Conference>();
            int recordCount = queryResult.Count();
            int mod;
            int totalPageCount;
            int offset;
            int recordToReturn;
            if (size == 0)
            {
                mod = 0;
                totalPageCount = 1;
                offset = 0;
                recordToReturn = recordCount;
            }
            else
            {
                mod = recordCount % size;
                totalPageCount = (recordCount/size) + (mod == 0 ? 0 : 1);
                offset = size * (page - 1);
                recordToReturn = size;
            }
            pageResult.Results = queryResult.OrderBy(c => c.Id)
                                            .Skip(offset)
                                            .Take(recordToReturn)
                                            .ToList();
            pageResult.PageCount = totalPageCount;
            pageResult.RecordCount = recordCount;
            return pageResult;
        }

        public Conference GetConferenceByTitle(string title)
        {
            var conference = appDbContext.Conferences
                                         .FirstOrDefault(e => string.Compare(e.Title, title, true) == 0);
            return conference;
        }


        public Conference UpdateConferenceInfo(int id, Conference conference, string base64Image)
        {
          conference.Id = id;
          conference.Rates.ToList().ForEach(x => x.conferenceId = id);
          conference.Banner = string.IsNullOrEmpty(base64Image) ? Enumerable.Empty<byte>().ToArray() : new ImageManager(base64Image).Bytes;
          var updatedConference = appDbContext.UpdateGraph<Conference>(conference, map => map.OwnedCollection(e => e.Rates));
          return updatedConference;
        }

        public ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}