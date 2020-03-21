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
    public class PromoRepository : Repository<Promo> , IPromoRepository
    {
        public PromoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public PageResult<Promo> GetPagedPromos(int page, int size)
        {
            PageResult<Promo> pageResult = new PageResult<Promo>();

            int offset = size * (page - 1);
            var recordCount = appDbContext.Promos.Count();
            var mod = recordCount % size;
            var totalPageCount = (recordCount / size) + (mod == 0 ? 0 : 1);

            pageResult.RecordCount = recordCount;
            pageResult.PageCount = totalPageCount;

            pageResult.Results =  appDbContext.Promos.OrderBy(p => p.Id)
                                              .Skip(offset)
                                              .Take(size)
                                              .ToList();

            return pageResult;
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