using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using RefactorThis.GraphDiff;
using AutoMapper;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories
{
    public class AccountRepository
    {
        private readonly ApplicationDbContext _context;
        private IUserStore<ApplicationUser, int> store;
        public UserManager<ApplicationUser, int> UserManager;
        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
            store = new CustomUserStore(_context);
            UserManager = new UserManager<ApplicationUser, int>(store);
        }

        public ApplicationUser UpdateAccount(int id, RequestAccountDTO newValues)
        {
            var user = UserManager.FindById(id);
            _context.Entry(user.UserInfo).CurrentValues.SetValues(newValues.UserInfo);
            if (user.PRCDetail == null && newValues.PRCDetail != null)
            {
                PRCDetail prcNew = new PRCDetail()
                {
                    Id = id,
                    IdNumber = newValues.PRCDetail.IdNumber,
                    ExpirationDate = DateTime.Parse(newValues.PRCDetail.ExpirationDate).Date
                };

                _context.Entry(prcNew).State = System.Data.Entity.EntityState.Added;
            }
            else if (user.PRCDetail != null && newValues.PRCDetail == null)
            {
                var obj = _context.PRCDetails.Find(id);
                _context.PRCDetails.Remove(obj);
            }

            else if(user.PRCDetail != null && newValues.PRCDetail != null)
            {
                _context.Entry(user.PRCDetail).CurrentValues.SetValues(newValues.PRCDetail);
            }
            return user;
        }
    }
}