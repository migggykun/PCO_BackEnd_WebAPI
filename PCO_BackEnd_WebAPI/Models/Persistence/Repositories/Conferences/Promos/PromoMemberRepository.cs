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
    public class PromoMemberRepository : Repository<PromoMember> , IPromoMemberRepository
    {
        public PromoMemberRepository(ApplicationDbContext context) : base(context)
        {

        }

        public PromoMember UpdatePromoMember(PromoMember memberToUpdate)
        {
            var updatedPromoMembers = appDbContext.UpdateGraph<PromoMember>(memberToUpdate);
            return updatedPromoMembers;
        }

        public List<PromoMember> AddPromoMembers(List<PromoMember> promoMembers)
        {
            var addedPromoMembers = appDbContext.PromoMembers.AddRange(promoMembers);
            return addedPromoMembers.ToList();
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