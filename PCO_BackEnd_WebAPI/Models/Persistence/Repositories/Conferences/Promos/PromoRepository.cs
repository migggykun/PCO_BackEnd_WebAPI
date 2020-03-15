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

        public Promo UpdatePromoDetails(int id, Promo promo)
        {
            var promoToUpdate = appDbContext.Promos.Find(id);
            var PromoMembersToUpdate = appDbContext.PromoMembers.Where(p => p.PromoId == id).ToList();
            var newValues = promo.PromoMembers.ToList();
            for (int x = 0; x < PromoMembersToUpdate.Count; x++)
            {
                PromoMembersToUpdate[x].MembershipTypeId = newValues[x].MembershipTypeId;
            }

            appDbContext.Entry(promoToUpdate).CurrentValues.SetValues(promo);
            return promoToUpdate;
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