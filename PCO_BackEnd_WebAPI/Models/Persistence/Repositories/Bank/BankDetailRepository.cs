using PCO_BackEnd_WebAPI.Models.Bank;
using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Bank
{
    public class BankDetailRepository : Repository<BankDetail> , IBankDetailRepository
    {
        public BankDetailRepository(ApplicationDbContext context):base(context)
        {

        }

        public BankDetail UpdateBankDetails(int id, BankDetail bankDetail)
        {
            bankDetail.Id = id;
            return appDbContext.UpdateGraph<BankDetail>(bankDetail);
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