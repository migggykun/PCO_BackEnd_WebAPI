using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Registrations;
namespace PCO_BackEnd_WebAPI.Models.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,CustomRole, 
                                        int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("somee_sample")
        {

        }

        public virtual DbSet<Conference> Conferences { get; set; }
        public virtual DbSet<DailyAttendanceRecord> DailyAttendanceRecords { get; set; }
        public virtual DbSet<MembershipType> MembershipTypes { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<PRCDetail> PRCDetails { get; set; }
        public virtual DbSet<PromoMember> PromoMembers { get; set; }
        public virtual DbSet<Promo> Promos { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<RegistrationPayment> RegistrationPayments { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }

        public static ApplicationDbContext Create()
        {
           return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //Load Identity relationships of identity classes (Ex. AspUsers, ASPRoles, etc)

            modelBuilder.Entity<ApplicationUser>().HasKey<int>(e => e.Id);

            modelBuilder.Entity<ApplicationUser>()
                        .HasRequired(e => e.UserInfo)
                        .WithRequiredPrincipal()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationUser>()
                        .HasOptional(e => e.PRCDetail)
                        .WithRequired()
                        .WillCascadeOnDelete(true);


            modelBuilder.Entity<Conference>()
                        .Property(e => e.Banner)
                        .IsFixedLength();
            
            modelBuilder.Entity<Conference>()
                        .HasOptional(e => e.Promo);

            modelBuilder.Entity<Conference>()
                        .HasMany(e => e.Rates)
                        .WithRequired()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Promo>()
                        .HasMany(p => p.PromoMembers)
                        .WithRequired()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Period>()
                        .Property(e => e.Name)
                        .IsFixedLength();

            modelBuilder.Entity<RegistrationPayment>()
                        .Property(e => e.proofOfPayment)
                        .IsFixedLength();

            modelBuilder.Entity<Registration>()
                        .HasMany(e => e.DailyAttendanceRecords)
                        .WithRequired(e => e.Registration)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Registration>()
                        .HasOptional(e => e.RegistrationPayment)
                        .WithRequired(e => e.Registration);

            modelBuilder.Entity<Registration>()
                        .HasRequired(e => e.Conference)
                        .WithMany()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Registration>()
                        .HasRequired(e => e.User)
                        .WithMany()
                        .WillCascadeOnDelete(true);
        }
    }
}