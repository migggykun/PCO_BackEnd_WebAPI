﻿using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using RefactorThis.GraphDiff;
using System.Threading.Tasks;
using PCO_BackEnd_WebAPI.Models.Pagination;
using System.Globalization;
using System.Data.Entity;
using PCO_BackEnd_WebAPI.Models.Helpers;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories
{
    public class AccountRepository
    {
        private readonly ApplicationDbContext _context;
        private CustomUserStore store;
        public UserManager<ApplicationUser, int> UserManager;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
            store = new CustomUserStore(_context);
            UserManager = new UserManager<ApplicationUser, int>(store);
        }

        public PageResult<ApplicationUser> GetPagedAccounts(int page, 
                                                            int size,
                                                            string filter = null)
        {
            PageResult<ApplicationUser> pageResult;
            DateTime PRCDate;
            DataConverter.ConvertToDateTime(filter, out PRCDate);
            IQueryable<ApplicationUser> queryResult = UserManager.Users.Where(u => (filter == null) ?
                                                              true
                                                              :
                                                              (u.UserInfo.FirstName.Contains(filter) ||
                                                               u.UserInfo.MiddleName.Contains(filter) ||
                                                               u.UserInfo.LastName.Contains(filter)) ||
                                                               u.UserInfo.Organization.Contains(filter) ||
                                                               u.UserInfo.MembershipType.Name.Contains(filter) ||
                                                               DbFunctions.TruncateTime(u.PRCDetail.ExpirationDate) == (DbFunctions.TruncateTime(PRCDate)) ||
                                                               u.PRCDetail.IdNumber.Contains(filter) ||
                                                               u.Email.Contains(filter) ||
                                                               u.PhoneNumber.Contains(filter));

            pageResult = PaginationManager<ApplicationUser>.GetPagedResult(queryResult, page, size, UserManager.Users.Count());
            return pageResult;
        }

        public ApplicationUser UpdateAccount(int id, ApplicationUser user)
        {
            var oldUser = UserManager.FindById(id);
            oldUser.UserName = user.Email;
            oldUser.PhoneNumber = user.PhoneNumber;
            oldUser.Email = user.Email;
            oldUser.IsAdmin = user.IsAdmin;
            oldUser.EmailConfirmed = (string.Compare(oldUser.Email, user.Email, true) == 0 ? true : false);

            //Update UserInfo object
            user.UserInfo.Id = id;
            UpdateUserInfo(oldUser.UserInfo, user.UserInfo);

            //Update PRCDetail Object
            UpdatePRCDetail(oldUser.PRCDetail, user.PRCDetail, id);

            return oldUser;
        }

        /// <summary>
        /// Updates UserInfo
        /// </summary>
        /// <param name="oldInfo"></param>
        /// <param name="newInfo"></param>
        private void UpdateUserInfo(UserInfo oldInfo, UserInfo newInfo)
        {
            newInfo.Id = oldInfo.Id;
            _context.Entry(oldInfo).CurrentValues.SetValues(newInfo);
        }

        /// <summary>
        /// Updates PRC Details
        /// </summary>
        /// <param name="oldPRC"></param>
        /// <param name="newPRC"></param>
        private void UpdatePRCDetail(PRCDetail oldPRC, PRCDetail newPRC, int id)
        {
            if (oldPRC == null && newPRC != null)
            {
                newPRC.Id = id;
                _context.Entry(newPRC).State = System.Data.Entity.EntityState.Added;
            }
            else if (oldPRC != null && newPRC == null)
            {
                oldPRC.Id = id;
                _context.Entry(oldPRC).State = System.Data.Entity.EntityState.Deleted;
            }

            else if (oldPRC != null && newPRC != null)
            {
                newPRC.Id = id;
                _context.Entry(oldPRC).CurrentValues.SetValues(newPRC);
            }
        }
    }
}