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

        public Promo UpdatePromoDetails(Promo promo)
        {
            var updatedPromo = appDbContext.UpdateGraph<Promo>(promo, map => map.OwnedCollection(p => p.PromoMembers));
            return updatedPromo;
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