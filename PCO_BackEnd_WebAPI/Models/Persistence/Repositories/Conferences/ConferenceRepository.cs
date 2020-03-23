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

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
        public ConferenceRepository(DbContext context) : base(context)
        {
              
        }

        public PageResult<Conference> GetPagedConferences(int page, int size, string filter = null)
        {
            PageResult<Conference> pageResult = new PageResult<Conference>();
            int recordCount = appDbContext.Conferences.Count();
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

            pageResult.RecordCount = recordCount;
            pageResult.PageCount = totalPageCount;
            pageResult.Results = appDbContext.Conferences.Where(c => string.IsNullOrEmpty(filter) ? true : c.Title.Contains(filter))
                                                          .OrderBy(c => c.Id)
                                                          .Skip(offset)
                                                          .Take(recordToReturn)
                                                          .ToList();
            return pageResult;
        }

        public Conference GetConferenceByTitle(string title)
        {
            var conference = appDbContext.Conferences
                                         .FirstOrDefault(e => string.Compare(e.Title, title, true) == 0);
            return conference;
        }


        public Conference UpdateConferenceInfo(int id, Conference conference)
        {
          conference.Id = id;
          conference.Rates.ToList().ForEach(x => x.conferenceId = id);
          var updatedConference = appDbContext.UpdateGraph<Conference>(conference, map => map.OwnedCollection(e => e.Rates));
          return updatedConference;
        }

        public List<Conference> GetUpcomingConferences(DateTime date)
        {
                return appDbContext.Conferences.Where(c => c.Start >= date.Date).ToList();
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