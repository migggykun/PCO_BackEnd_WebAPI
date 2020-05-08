using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PCO_BackEnd_WebAPI.Security.Cryptography.Models;

namespace PCO_BackEnd_WebAPI.Security.Cryptography.Entity
{
    public partial class ClientEntity : DbContext
    {
        public ClientEntity()
            : base("name=ClientEntity")
        {
        }

        public virtual DbSet<ClientInfo> ClientInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
