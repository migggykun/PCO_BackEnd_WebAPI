using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PCO_BackEnd_WebAPI.Security.Models;

namespace PCO_BackEnd_WebAPI.Security.Entity
{
    public partial class ClientCredentialsContext : DbContext
    {
        public ClientCredentialsContext()
            : base("name=ClientCredentialsContext")
        {
        }

        public virtual DbSet<ClientCredential> ClientCredentials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}