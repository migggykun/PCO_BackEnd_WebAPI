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
            PageResult<PromoMember> pageResult = new PageResult<PromoMember>();
            int recordCount = appDbContext.PromoMembers.Count();
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
                totalPageCount = (recordCount / size) + (mod == 0 ? 0 : 1);
                offset = size * (page - 1);
                recordToReturn = size;
            }
            pageResult.RecordCount = recordCount;
            pageResult.PageCount = totalPageCount;

            pageResult.Results = appDbContext.PromoMembers.OrderBy(p => p.Id)
                                              .Skip(offset)
                                              .Take(recordToReturn)
                                              .ToList();

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

        public ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}