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
using PCO_BackEnd_WebAPI.Models.Images.Manager;

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

            IQueryable<Conference> queryResult = appDbContext.Conferences.OrderBy(x=>x.Start).ThenBy(y=>y.Id)
                
                                                         .Where(c => string.IsNullOrEmpty(filter) ? true : c.Title.Contains(filter) ||
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
            pageResult = PaginationManager<Conference>.GetUnorderedPageResult(queryResult, page, size);
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
          conference.ConferenceDays.ToList().ForEach(x => x.ConferenceActivities.ToList().ForEach(y => y.ActivityRates.ToList().ForEach(z => z.conferenceId = id)));

            //Update banner
            var banner = appDbContext.Banners.Find(id);
            if (base64Image != null)
            {
                if (banner == null)
                {
                    banner = new Banner();
                    banner.Id = id;
                    appDbContext.Banners.Add(banner);
                }
                ImageManager bannerManager = new ImageManager(base64Image);
                banner.Image = bannerManager.Bytes;
            }

            var updatedConference = appDbContext.UpdateGraph<Conference>(conference, map => map.OwnedCollection(a => a.ConferenceDays,aa=>aa.OwnedCollection(b=>b.ConferenceActivities,bb=>bb.OwnedCollection(c=>c.ActivityRates).OwnedEntity(c => c.ActivitySchedule))));
            
            //Update PackageRates
            var packageRates = new RateRepository(appDbContext).UpdateRates(conference.Rates, id);
          return updatedConference;     
        }

        /// <summary>
        /// helper Function for finding conference activity by id to populate activity in conferenceDTO
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        public void FillInConferenceActivities(Conference conference)
        {
            var activities = appDbContext.Activities.ToList();

            foreach (ConferenceDay conferenceDay in conference.ConferenceDays)
            {
                foreach (ConferenceActivity conferenceActivity in conferenceDay.ConferenceActivities.Where(x => x.ActivitySchedule.Activity == null))
                {
                    conferenceActivity.ActivitySchedule.Activity = activities.Find(x => x.Id == conferenceActivity.ActivitySchedule.ActivityId);
                }

            }
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