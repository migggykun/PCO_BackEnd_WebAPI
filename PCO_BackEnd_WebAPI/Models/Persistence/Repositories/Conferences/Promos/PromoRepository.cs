using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences.Promos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences.Promos
{
    public class PromoRepository : Repository<Promo> , IPromoRepository
    {
        public PromoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public Promo UpdatePromoDetails(int id, Promo promoUpdate)
        {
            promoUpdate.Id = id;
            var oldPromo = appDbContext.Promos.Find(id);
            PromoMember result;

            //assign primary key if exists, otherwise 0;
            foreach (var p in promoUpdate.PromoMembers)
            {
                var it = oldPromo.PromoMembers.ToList().Where(x => x.PromoId == id &&
                                                       x.MembershipTypeId == p.MembershipTypeId).FirstOrDefault();
                
                p.Id = it == null ? 0 : it.Id;
            }

            //Sets promo Id for each promo Members 
            promoUpdate.PromoMembers.ToList().ForEach(x => x.PromoId = id);


            appDbContext.UpdateGraph<Promo>(promoUpdate, p => p.OwnedCollection(pm => pm.PromoMembers));
            return promoUpdate;
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