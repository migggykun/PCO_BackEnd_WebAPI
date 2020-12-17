using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Images.Manager;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using RefactorThis.GraphDiff;
using System;
using System.Data.Entity;
using System.Linq;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    /// <summary>
    /// Database Query Interface for Conference Table
    /// </summary>
    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="context"></param>
        public ConferenceRepository(DbContext context) : base(context)
        {
              
        }

        /// <summary>
        /// Get Paged Results of query from all conferences using filters. all if none.
        /// </summary>
        /// <param name="page">number of pages</param>
        /// <param name="size">number of items per page</param>
        /// <param name="filter">search keyword</param>
        /// <param name="day">filter by day</param>
        /// <param name="month">filter by month</param>
        /// <param name="year">filter by year</param>
        /// <param name="fromDate">filter by from</param>
        /// <param name="toDate">filter by to</param>
        /// <returns></returns>
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

        /// <summary>
        /// get conference by title
        /// </summary>
        /// <param name="title">title of conference</param>
        /// <returns></returns>
        public Conference GetConferenceByTitle(string title)
        {
            var conference = appDbContext.Conferences
                                         .FirstOrDefault(e => string.Compare(e.Title, title, true) == 0);
            return conference;
        }

        /// <summary>
        /// Update Conference Information
        /// </summary>
        /// <param name="id">id of conference</param>
        /// <param name="conference">name of conference</param>
        /// <param name="base64Image">banner image of conference</param>
        /// <returns></returns>
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
        /// Helper Method to fillin activities after conference is added.
        /// </summary>
        /// <param name="conference">conference to fill activities to</param>
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