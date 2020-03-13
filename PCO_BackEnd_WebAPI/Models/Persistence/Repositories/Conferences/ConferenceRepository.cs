using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Conferences;
using System.Data.Entity;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using RefactorThis.GraphDiff;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
        public ConferenceRepository(DbContext context) : base(context)
        {
              
        }

        public Conference GetConferenceByTitle(string title)
        {
            var conference = appDbContext.Conferences
                                         .FirstOrDefault(e => string.Compare(e.Title, title, true) == 0);
            return conference;
        }


        public Conference UpdateConferenceInfo(Conference conference)
        {
          var updatedConference = appDbContext.UpdateGraph<Conference>(conference, map => map.OwnedCollection(e => e.Rates)
                                                                                             .OwnedEntity(c => c.Promo, with =>
                                                                                             with.OwnedCollection(p => p.PromoMembers)));
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