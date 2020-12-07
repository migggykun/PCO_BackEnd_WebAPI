using PCO_BackEnd_WebAPI.Models.PCOAdmin;
using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;
using System.Data.Entity;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.PCOAdmin
{
    public class PCOAdminRepository : Repository<PCOAdminDetail>, IPCOAdminRepository
    {
        public PCOAdminRepository(DbContext Context) : base(Context)
        {
        }

        public double GetAnnualFee()
        {
            return appDbContext.PCOAdminDetails.FirstOrDefault().AnnualMembershipFee;
        }

        public string GetPassword()
        {
            return appDbContext.PCOAdminDetails.FirstOrDefault().WebsitePassword;
        }

        public PCOAdminDetail UpdatePCOAdminDetails(double? price = null, string password = null)
        {
            PCOAdminDetail update = new PCOAdminDetail();
            if (price == null && password == null) return null;
            if (price != null)
            {
                update.AnnualMembershipFee = price.Value;
                update.WebsitePassword = appDbContext.PCOAdminDetails.FirstOrDefault().WebsitePassword;
            }
            if (password != null)
            {
                update.WebsitePassword = password;
                update.AnnualMembershipFee = appDbContext.PCOAdminDetails.FirstOrDefault().AnnualMembershipFee;
            }
            update.Id = appDbContext.PCOAdminDetails.FirstOrDefault().Id;
            return appDbContext.UpdateGraph<PCOAdminDetail>(update);
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