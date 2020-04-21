using PCO_BackEnd_WebAPI.Models.Bank;
using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Bank
{
    public class BankDetailRepository : Repository<BankDetail> , IBankDetailRepository
    {
        public BankDetailRepository(ApplicationDbContext context):base(context)
        {

        }

        public PageResult<BankDetail> GetPagedBankDetails(int page, int size, string filter = null)
        {
            PageResult<BankDetail> pageResult;

            IQueryable<BankDetail> queryResult = appDbContext.BankDetails.Where(b => string.IsNullOrEmpty(filter) ? true :
                                                                                b.AccountName.Contains(filter) ||
                                                                                b.BankName.Contains(filter) ||
                                                                                b.AccountNumber.Contains(filter));

            pageResult = PaginationManager<BankDetail>.GetPagedResult(queryResult, page, size);
            return pageResult;
        }

        public BankDetail UpdateBankDetails(int id, BankDetail bankDetail)
        {
            bankDetail.Id = id;
            return appDbContext.UpdateGraph<BankDetail>(bankDetail);
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