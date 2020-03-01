using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ConferenceRepository Conferences { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Conferences = new ConferenceRepository(_context);
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