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
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public PageResult<Address> GetPagedAddress(int page, int size)
        {
            PageResult<Address> pageResult;
            IQueryable<Address> queryResult = appDbContext.Addresses;

            pageResult = PaginationManager<Address>.GetPagedResult(queryResult, page, size);
            return pageResult;
        }

        public Address UpdateAddress(int id, Address entityToUpdate)
        {
            entityToUpdate.Id = id;
            return appDbContext.UpdateGraph<Address>(entityToUpdate);
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