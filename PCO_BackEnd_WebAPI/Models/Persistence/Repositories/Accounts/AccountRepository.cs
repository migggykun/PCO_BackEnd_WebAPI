using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Accounts;

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

        public ApplicationUser UpdateAccount(ApplicationUser oldValues, RequestAccountDTO newValues)
        {
            oldValues.Email = newValues.Email;
            oldValues.PhoneNumber = newValues.PhoneNumber;
            _context.Entry(oldValues.PRCDetail).CurrentValues.SetValues(newValues.PRCDetail);
            _context.Entry(oldValues.UserInfo).CurrentValues.SetValues(newValues.UserInfo);
            return oldValues;
        }
    }
}