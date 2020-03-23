﻿using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts
{
    public interface IUserInfoRepository : IRepository<UserInfo>
    {
        PageResult<UserInfo> GetPagedUserInfo(int page, int size);
        UserInfo UpdateUserInfo(int id, UserInfo entityToUpdate);
    }
}