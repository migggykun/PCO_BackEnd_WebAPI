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
    public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public PageResult<UserInfo> GetPagedUserInfo(int page, int size)
        {
            PageResult<UserInfo> pageResult;
            IQueryable<UserInfo> queryResult = appDbContext.UserInfos;

            pageResult = PaginationManager<UserInfo>.GetPagedResult(queryResult, page, size);
            return pageResult;
        }

        public UserInfo UpdateUserInfo(int id, UserInfo entityToUpdate)
        {
            entityToUpdate.Id = id; 
            return appDbContext.UpdateGraph<UserInfo>(entityToUpdate);
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