using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts
{
    public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public UserInfo UpdateUserInfo(int id, UserInfo entityToUpdate)
        {
            entityToUpdate.Id = id; 
            return appDbContext.UpdateGraph<UserInfo>(entityToUpdate);
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