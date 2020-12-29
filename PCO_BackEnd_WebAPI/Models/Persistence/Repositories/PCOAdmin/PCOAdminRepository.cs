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

        public double GetAnnualFee(bool renew)
        {
            var pcoAdmin = appDbContext.PCOAdminDetails.FirstOrDefault();
            return renew?pcoAdmin.AnnualMembershipFee:pcoAdmin.PcoMembershipFee;
        }

        public string GetPassword()
        {
            return appDbContext.PCOAdminDetails.FirstOrDefault().WebsitePassword;
        }

        public PCOAdminDetail UpdatePCOAdminDetails(double? reg = null, double? renew = null, string password = null)
        {
            PCOAdminDetail update = new PCOAdminDetail();
            if (reg == null && renew ==null && password == null) return null;
            var pcoAdmin = appDbContext.PCOAdminDetails.FirstOrDefault();
           
            update.PcoMembershipFee = reg!=null?reg.Value:pcoAdmin.PcoMembershipFee;
            update.AnnualMembershipFee = renew!=null?renew.Value:pcoAdmin.AnnualMembershipFee;
            update.WebsitePassword = password != null ? password: pcoAdmin.WebsitePassword;

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