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
using PCO_BackEnd_WebAPI.Models.Bank;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.PCOAdmin;

namespace PCO_BackEnd_WebAPI.Models.Entities
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,CustomRole, 
                                        int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("PCO_Context")
        {
            //Database.SetInitializer(new ApplicationUserSeeder());
        }

        public virtual DbSet<Conference> Conferences { get; set; }
        public virtual DbSet<DailyAttendanceRecord> DailyAttendanceRecords { get; set; }
        public virtual DbSet<MembershipType> MembershipTypes { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<PRCDetail> PRCDetails { get; set; }
        public virtual DbSet<PromoMember> PromoMembers { get; set; }
        public virtual DbSet<Promo> Promos { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<MemberRegistration> MemberRegistrations { get; set; }
        public virtual DbSet<RegistrationStatus> RegistrationStatus { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<BankDetail> BankDetails { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ActivitySchedule> ActivitySchedules { get; set; }
        public virtual DbSet<ConferenceDay> ConferenceDays { get; set; }
        public virtual DbSet<ConferenceActivity> ConferenceActivities { get; set; }

        public virtual DbSet<ActivityAttendance> ActivityAttendances { get; set; }

        public virtual DbSet<ActivitiesToAttend> ActivitiesToAttend { get; set; }
        public virtual DbSet<Member> Members { get; set; }

        public virtual DbSet<PCOAdminDetail> PCOAdminDetails{ get; set; }

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

            modelBuilder.Entity<UserInfo>()
                        .HasRequired(e => e.Address)
                        .WithRequiredPrincipal()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationUser>()
                        .HasOptional(e => e.PRCDetail)
                        .WithOptionalPrincipal()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<UserInfo>()
                        .HasRequired(u => u.MembershipType)
                        .WithMany();

            modelBuilder.Entity<Conference>()
                        .HasOptional(e => e.Promo);

            modelBuilder.Entity<Conference>()
                        .HasMany(e => e.Rates)
                        .WithRequired()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Conference>()
                        .HasOptional(e => e.Banner)
                        .WithRequired()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Promo>()
                        .HasMany(p => p.PromoMembers)
                        .WithRequired()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<PromoMember>()
                        .HasRequired(u => u.MembershipType)
                        .WithMany();

            modelBuilder.Entity<Period>()
                        .Property(e => e.Name)
                        .IsFixedLength();

            modelBuilder.Entity<Registration>()
                        .HasMany(e => e.ActivitiesToAttend);
                        //.WithRequired(e => e.Registration)
                        //.WillCascadeOnDelete(false);

            modelBuilder.Entity<Registration>()
                        .HasRequired(e => e.User)
                        .WithMany()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Registration>()
                        .HasRequired(e => e.Promo)
                        .WithMany()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Registration>()
                        .HasRequired(e => e.User)
                        .WithMany();

            modelBuilder.Entity<MemberRegistration>()
                        .HasRequired(e => e.User)
                        .WithMany();

            modelBuilder.Entity<Payment>()
                        .HasRequired(e => e.Receipt)
                        .WithRequiredPrincipal()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Payment>()
                        .HasOptional(e => e.Registration)
                        .WithMany();

            modelBuilder.Entity<Payment>()
                        .HasOptional(e => e.MemberRegistration)
                        .WithMany();

            modelBuilder.Entity<Conference>()
                        .HasMany(e => e.ConferenceDays)
                        .WithRequired()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<ConferenceDay>()
                        .HasMany(e => e.ConferenceActivities)
                        .WithRequired()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<ConferenceActivity>()
                        .HasRequired(e => e.ActivitySchedule);

            modelBuilder.Entity<ConferenceActivity>()
                        .HasMany(e => e.ActivitiesToAttend)
                        .WithOptional()
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<ActivitySchedule>()
                        .HasRequired(e => e.Activity);

            modelBuilder.Entity<ActivityAttendance>();

            modelBuilder.Entity<ActivitiesToAttend>();
                        //.HasRequired(u=>u.ConferenceActivityId)
                        //.WithMany();

            modelBuilder.Entity<Member>();

            modelBuilder.Entity<PCOAdminDetail>();
        }
    }
}