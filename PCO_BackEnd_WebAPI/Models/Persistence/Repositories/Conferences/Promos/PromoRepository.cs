using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences.Promos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Helpers;
namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences.Promos
{
    public class PromoRepository : Repository<Promo> , IPromoRepository
    {
        public PromoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public PageResult<Promo> GetPagedPromos(int page, int size, string filter)
        {
            PageResult<Promo> pageResult = new PageResult<Promo>();
            int recordCount = appDbContext.Promos.Count();
            int amount;
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
            Int32.TryParse(filter, out amount);
            pageResult.Results = appDbContext.Promos.OrderBy(p => p.Id)
                                              .Where(x => string.IsNullOrEmpty(filter) ? true : x.Name.Contains(filter) || 
                                                                                                x.Description.Contains(filter) ||
                                                                                                x.PromoMembers.Any(p => p.MembershipType.Name.Contains(filter)) ||
                                                                                                x.Amount == amount)
                                              .Skip(offset)
                                              .Take(recordToReturn)
                                              .ToList();
            pageResult.PageCount = totalPageCount;
            pageResult.RecordCount = recordCount;
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