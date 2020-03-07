﻿using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts;

namespace PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ConferenceRepository Conferences { get; set; }
        public IMembershipTypeRepository MembershipTypes { get; set; }
        public IPRCDetailRepository PRCDetails { get; set; }
        public IUserInfoRepository UserInfos { get; set; }
        public IMembershipAssignmentRepository MembershipAssignments { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Conferences = new ConferenceRepository(_context);
            MembershipTypes = new MembershipTypeRepository(_context);
            PRCDetails = new PRCDetailRepository(_context);
            UserInfos = new UserInfoRepository(_context);
            MembershipAssignments = new MembershipAssignmentRepository(_context);
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