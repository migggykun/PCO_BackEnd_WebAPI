using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences.Promos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences.Promos
{
    public class PromoMemberRepository : Repository<PromoMember> , IPromoMemberRepository
    {
        public PromoMemberRepository(ApplicationDbContext context) : base(context)
        {

        }

        public PageResult<PromoMember> GetPagedPromoMember(int page, int size)
        {
            PageResult<PromoMember> pageResult;
            IQueryable<PromoMember> queryResult = appDbContext.PromoMembers;

            pageResult = PaginationManager<PromoMember>.GetPagedResult(queryResult, page, size);
            return pageResult;
            
        }

        public PromoMember UpdatePromoMember(int id, PromoMember memberToUpdate)
        {
            var promoMember = appDbContext.PromoMembers.Find(id);
            appDbContext.Entry(promoMember).CurrentValues.SetValues(memberToUpdate);
            return promoMember;
        }

        public List<PromoMember> AddPromoMembers(List<PromoMember> promoMembers)
        {
            var addedPromoMembers = appDbContext.PromoMembers.AddRange(promoMembers);
            return addedPromoMembers.ToList();
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