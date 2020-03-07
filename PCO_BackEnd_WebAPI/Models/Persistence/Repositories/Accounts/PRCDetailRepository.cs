using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts
{
    public class PRCDetailRepository : Repository<PRCDetail> , IPRCDetailRepository
    {
        public PRCDetailRepository(ApplicationDbContext context) : base(context)
        {

        }

        public PRCDetail GetPRCDetailById(string prcId)
        {
            return appDbContext.PRCDetails.FirstOrDefault(e => string.Compare(e.prcId, prcId, false) == 0);
            
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