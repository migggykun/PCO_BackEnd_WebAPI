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
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System.Data.Entity.Validation;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Registrations;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Registrations;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Bank;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Attendance;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.PCOAdmin;

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
        public AccountRepository Accounts { get; set; }
        public IRegistrationRepository Registrations{ get; set; }
        public IMemberRegistrationRepository MemberRegistrations { get; set; }
        public IPaymentRepository Payments { get; set; }
        public Repository<RegistrationStatus> RegStatus { get; set; }
        public BankDetailRepository BankDetails { get; set; }
        public IAddressRepository Addresses { get; set; }
        public Repository<Banner> Banners { get; set; }
        public Repository<Receipt> Receipts { get; set; }
        public IActivityRepository Activities { get; set; }

        //test
        public IActivityScheduleRepository ActivitySchedules { get; set; }

        public AttendanceRepository ActivityAttendances { get; set; }
        public IMemberRepository Members { get; set; }

        public IPCOAdminRepository PCOAdminDetail { get; set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Accounts = new AccountRepository(_context);
            Conferences = new ConferenceRepository(_context);
            MembershipTypes = new MembershipTypeRepository(_context);
            PRCDetails = new PRCDetailRepository(_context);
            UserInfos = new UserInfoRepository(_context);
            Rates = new RateRepository(_context);
            Promos = new PromoRepository(_context);
            PromoMembers = new PromoMemberRepository(_context);
            Registrations = new ConferenceRegistrationRepository(_context);
            MemberRegistrations = new MemberRegistrationRepository(_context);
            Payments = new PaymentRepository(_context);
            RegStatus = new Repository<RegistrationStatus>(_context);
            BankDetails = new BankDetailRepository(_context);
            Addresses = new AddressRepository(_context);
            Banners = new Repository<Banner>(_context);
            Receipts = new Repository<Receipt>(_context);
            Activities = new ActivityRepository(_context);
            //test
            ActivitySchedules = new ActivityScheduleRepository(_context);
            ActivityAttendances = new AttendanceRepository(_context);
            Members = new MemberRepository(_context);
            PCOAdminDetail = new PCOAdminRepository(_context);
        }

        /// <summary>
        /// Reflect changes to database
        /// </summary>
        /// <returns></returns>
        public int Complete()
        {
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                return _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }


        //Disconnects connection with external resources
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}