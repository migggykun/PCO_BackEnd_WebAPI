using PCO_BackEnd_WebAPI.Models.Entities;
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

        public ApplicationUser FindById(int id)
        {
            ApplicationUser result = UserManager.Users.ToList().Find(x => x.Id == id);

            return result;
        }

        public ApplicationUser GetUserByPhoneNumber(string phoneNumber)
        {
            ApplicationUser result = UserManager.Users.ToList().Find(x => x.PhoneNumber == phoneNumber);

            return result;
        }

        public PageResult<ApplicationUser> GetPagedAccounts(int page,
                                                            int size,
                                                            string filter = null,
                                                            string school = null,
                                                            string yearGraduated = null,
                                                            string organization = null,
                                                            string address = null,
                                                            string membershipType = null,
                                                            string pcoMembership = null,
                                                            DateTime? prcRegDateFrom = null,
                                                            DateTime? prcRegDateTo = null,
                                                            DateTime? prcExpDateFrom = null,
                                                            DateTime? prcExpDateTo = null,
                                                            DateTime? birthdayDateFrom = null,
                                                            DateTime? birthdayDateTo = null)
        {
            PageResult<ApplicationUser> pageResult;

            IQueryable<ApplicationUser> queryResult = UserManager.Users.Where(u => (filter == null) ?
                                                              true
                                                              :
                                                              (u.UserInfo.FirstName.Contains(filter) ||
                                                               u.UserInfo.MiddleName.Contains(filter) ||
                                                               u.UserInfo.LastName.Contains(filter)) ||
                                                               u.UserInfo.Organization.Contains(filter) ||
                                                               u.UserInfo.MembershipType.Name.Contains(filter) ||
                                                               u.PRCDetail.IdNumber.Contains(filter) ||
                                                               u.Email.Contains(filter) ||
                                                               u.PhoneNumber.Contains(filter));
            //Filter by organization
            queryResult = queryResult.Where(u => (organization == null) ?
                                                  true
                                                  :
                                                  (u.UserInfo.Organization.Contains(organization)));
            //Filter by membershipType
            queryResult = queryResult.Where(u => (membershipType == null) ?
                                                  true
                                                  :
                                                  (u.UserInfo.MembershipType.Name.Contains(membershipType)));

            //Filter by school
            queryResult = queryResult.Where(u => (string.IsNullOrEmpty(school)) ?
                                                  true
                                                  :
                                                  (u.UserInfo.School.Contains(school)));

            //Filter by year graduated
            queryResult = queryResult.Where(u => (string.IsNullOrEmpty(yearGraduated)) ?
                                                  true
                                                  :
                                                  (u.UserInfo.YearGraduated.Contains(yearGraduated)));
            //Filter by Address
            queryResult = queryResult.Where(u => (address == null) ?
                                       true
                                       :
                                       (u.UserInfo.Address.StreetAddress.Contains(address)) ||
                                       (u.UserInfo.Address.Barangay.Contains(address)) ||
                                       (u.UserInfo.Address.City.Contains(address)) ||
                                       (u.UserInfo.Address.Province.Contains(address)) ||
                                       (u.UserInfo.Address.Zipcode.Contains(address)) );

            //apply Date Filters
            queryResult = queryResult.Where(x =>
                (prcRegDateFrom != null && prcRegDateTo != null) ?
                (x.PRCDetail.RegistrationDate >= prcRegDateFrom &&
                x.PRCDetail.RegistrationDate <= prcRegDateTo)
                :
                true)
            .Where(x =>
                (prcExpDateFrom != null && prcExpDateTo != null) ?
                (x.PRCDetail.ExpirationDate >= prcExpDateFrom &&
                x.PRCDetail.ExpirationDate <= prcExpDateTo)
                :
                true)
            .Where(x =>
                (birthdayDateFrom != null && birthdayDateTo != null) ?
                (x.UserInfo.Birthday >= birthdayDateFrom &&
                x.UserInfo.Birthday <= birthdayDateTo)
                :
                true);

            if(!string.IsNullOrEmpty(pcoMembership))
            {
                if (pcoMembership.ToLower().Equals("active"))
                {
                    queryResult = queryResult.Where(u => u.IsMember == true && u.IsActive == true);
                }
                else if (pcoMembership.ToLower().Equals("inactive"))
                {
                    queryResult = queryResult.Where(u => u.IsMember == true && u.IsActive == false);
                }
                else if (pcoMembership.ToLower().Equals("non-member"))
                {
                    queryResult = queryResult.Where(u => u.IsMember == false);
                }
                else
                {
                    //do nothing
                }
            }
            else
            {
                //do nothing
            }
            

            pageResult = PaginationManager<ApplicationUser>.GetPagedResult(queryResult, page, size);
            return pageResult;
        }

        public ApplicationUser UpdateAccount(int id, ApplicationUser user)
        {
            var oldUser = UserManager.FindById(id);
            oldUser.UserName = user.Email;
            oldUser.PhoneNumber = user.PhoneNumber;
            oldUser.Email = user.Email;
            oldUser.IsAdmin = user.IsAdmin;
            oldUser.EmailConfirmed = (string.Compare(oldUser.Email, user.Email, true) == 0 ? oldUser.EmailConfirmed : false); 
            oldUser.PhoneNumberConfirmed = (string.Compare(oldUser.PhoneNumber, user.PhoneNumber, true) == 0 ? oldUser.PhoneNumberConfirmed : false); 

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
            newInfo.Address.Id = oldInfo.Address.Id;
            _context.Entry(oldInfo.Address).CurrentValues.SetValues(newInfo.Address);
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