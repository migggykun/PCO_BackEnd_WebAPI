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
            PageResult<Address> pageResult = new PageResult<Address>();
            int recordCount;
            if (appDbContext.Addresses.ToList() != null)
            {
                recordCount = appDbContext.Addresses.Count();
            }
            else
            {
                recordCount = 0;
            }
            
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
            pageResult.Results = appDbContext.Addresses
                                             .OrderBy(a => a.Id)
                                             .Skip(offset)
                                             .Take(recordToReturn)
                                             .ToList();
            pageResult.PageCount = totalPageCount;
            pageResult.RecordCount = recordCount;
            return pageResult;
        }

        public Address UpdateAddress(int id, Address entityToUpdate)
        {
            entityToUpdate.Id = id;
            return appDbContext.UpdateGraph<Address>(entityToUpdate);
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