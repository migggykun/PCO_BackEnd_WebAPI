using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts
{
    public class PRCDetailRepository : Repository<PRCDetail> , IPRCDetailRepository
    {
        public PRCDetailRepository(ApplicationDbContext context) : base(context)
        {

        }

        public PageResult<PRCDetail> GetPagedPRCDetail(int page, 
                                                       int size, 
                                                       string filter,
                                                       DateTime? aExpirationDateFrom = null,
                                                       DateTime? aExpirationDateTo = null)
        {
            PageResult<PRCDetail> pageResult = new PageResult<PRCDetail>();
            int recordCount = appDbContext.PRCDetails
                                             .Where(p =>
                                                        (String.IsNullOrEmpty(filter) == true) ?
                                                             true
                                                             :
                                                             p.IdNumber.Contains(filter))
                                             .Where(p =>
                                                        (aExpirationDateFrom == null &&
                                                         aExpirationDateTo == null) ?
                                                             true
                                                             :
                                                             (p.ExpirationDate >= aExpirationDateFrom &&
                                                              p.ExpirationDate <= aExpirationDateTo))
                                             .Count();
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
            pageResult.Results = appDbContext.PRCDetails
                                             .Where(p =>
                                                        (String.IsNullOrEmpty(filter) == true) ? 
                                                             true 
                                                             : 
                                                             p.IdNumber.Contains(filter))
                                             .Where(p =>
                                                        (aExpirationDateFrom == null &&
                                                         aExpirationDateTo == null) ? 
                                                             true 
                                                             :
                                                             (p.ExpirationDate >= aExpirationDateFrom &&
                                                              p.ExpirationDate <= aExpirationDateTo))
                                             .OrderBy(a => a.Id)
                                             .Skip(offset)
                                             .Take(recordToReturn)
                                             .ToList();
            pageResult.PageCount = totalPageCount;
            pageResult.RecordCount = recordCount;
            return pageResult;
        }

        public PRCDetail GetPRCDetailById(string prcId)
        {
            return appDbContext.PRCDetails.FirstOrDefault(e => string.Compare(e.IdNumber, prcId, false) == 0);
            
        }

        public PRCDetail Update(int id, PRCDetail entityToUpdate)
        {
            entityToUpdate.Id = id;
            return appDbContext.UpdateGraph<PRCDetail>(entityToUpdate);
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