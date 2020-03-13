using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences.Promos;

namespace PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ConferenceRepository Conferences { get; set; }
        public IMembershipTypeRepository MembershipTypes { get; set; }
        public IPRCDetailRepository PRCDetails { get; set; }
        public IUserInfoRepository UserInfos { get; set; }
        public IRateRepository Rates { get; set; }
        public IPromoRepository Promos { get; set; }
        public IPromoMemberRepository PromoMembers { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Conferences = new ConferenceRepository(_context);
            MembershipTypes = new MembershipTypeRepository(_context);
            PRCDetails = new PRCDetailRepository(_context);
            UserInfos = new UserInfoRepository(_context);
            Rates = new RateRepository(_context);
            Promos = new PromoRepository(_context);
            PromoMembers = new PromoMemberRepository(_context);
        }

        /// <summary>
        /// Reflect changes to database
        /// </summary>
        /// <returns></returns>
        public int Complete()
        {
                return _context.SaveChanges();
        }



        //Disconnects connection with external resources
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}