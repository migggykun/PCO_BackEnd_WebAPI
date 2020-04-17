using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts
{
    public class MembershipTypeRepository : Repository<MembershipType>, IMembershipTypeRepository
    {
        public MembershipTypeRepository(ApplicationDbContext context) : base(context)
        {

        }

        public PageResult<MembershipType> GetPagedMembershipTypes(int page, int size, string filter)
        {
            PageResult<MembershipType> pageResult = new PageResult<MembershipType>();
            IQueryable<MembershipType> queryResult = appDbContext.MembershipTypes.Where(u => string.IsNullOrEmpty(filter) ? true :
                                                                                                                          u.Name.Contains(filter) ||
                                                                                                                          u.Description.Contains(filter));
            int recordCount = queryResult.Count();
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
            pageResult.Results =  queryResult.OrderBy(a => a.Id)
                                             .Skip(offset)
                                             .Take(recordToReturn)
                                             .ToList();
            pageResult.PageCount = totalPageCount;
            pageResult.RecordCount = recordCount;
            return pageResult;
        }
        public MembershipType GetMembershipTypeByName(string membershipTypeName)
        {
            var membershipType = appDbContext.MembershipTypes
                                             .FirstOrDefault(e => string.Compare(e.Name, membershipTypeName, true) == 0);
            return membershipType;
        }

        public MembershipType UpdateMembershipType(int id, MembershipType entityToUpdate)
        {
            entityToUpdate.Id = id;
            return appDbContext.UpdateGraph<MembershipType>(entityToUpdate);
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