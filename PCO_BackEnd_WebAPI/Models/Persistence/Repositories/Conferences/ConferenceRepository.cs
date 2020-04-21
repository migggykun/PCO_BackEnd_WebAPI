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
using PCO_BackEnd_WebAPI.Models.Helpers;

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
            DateTime startDate;
            DateTime endDate;
            bool isStartDateValid = DataConverter.ConvertToDateTime(fromDate, out startDate);
            bool isEndDateValid = DataConverter.ConvertToDateTime(toDate, out endDate);

            IQueryable<Conference> queryResult = appDbContext.Conferences.Where(c => string.IsNullOrEmpty(filter) ? true : c.Title.Contains(filter) ||
                                                                                                                           c.Description.Contains(filter) ||
                                                                                                                           c.Location.Contains(filter))
                                                         .Where(c => string.IsNullOrEmpty(day) ? true : c.Start.Day.ToString().Contains(day)
                                                                || c.End.Day.ToString().Contains(day))
                                                         .Where(c => string.IsNullOrEmpty(month) ? true : c.Start.Month.ToString().Contains(month)
                                                                || c.End.Month.ToString().Contains(month))
                                                         .Where(c => string.IsNullOrEmpty(year) ? true : c.Start.Year.ToString().Contains(year)
                                                                || c.End.Year.ToString().Contains(year))
                                                         .Where(p => (string.IsNullOrEmpty(toDate) || !isEndDateValid)  ? true : DbFunctions.TruncateTime(p.End) <= (DbFunctions.TruncateTime(endDate)))
                                                         .Where(p => (string.IsNullOrEmpty(fromDate) || !isStartDateValid) ? true : DbFunctions.TruncateTime(p.Start) >= (DbFunctions.TruncateTime(startDate)));

            PageResult<Conference> pageResult;
            pageResult = PaginationManager<Conference>.GetPagedResult(queryResult, page, size, appDbContext.Conferences.Count());
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
          conference.Banner = string.IsNullOrEmpty(base64Image) ? null : new ImageManager(base64Image).Bytes;
          var updatedConference = appDbContext.UpdateGraph<Conference>(conference, map => map.OwnedCollection(e => e.Rates));
          return updatedConference;
        }

        private ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}