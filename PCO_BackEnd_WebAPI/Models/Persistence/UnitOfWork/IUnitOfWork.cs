using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Registrations;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Bank;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        AccountRepository Accounts { get; set; }
        ConferenceRepository Conferences { get; set; }
        Repository<RegistrationStatus> RegStatus { get; set; }
        BankDetailRepository BankDetails { get; set; }
        Repository<Banner> Banners { get; set; }
        Repository<Receipt> Receipts { get; set; }
        IMembershipTypeRepository MembershipTypes { get; set; }
        IPRCDetailRepository PRCDetails { get; set; }
        IUserInfoRepository UserInfos { get; set; }
        IAddressRepository Addresses { get; set; }
        IRateRepository Rates { get; set; }
        IPromoRepository Promos { get; set; }
        IPromoMemberRepository PromoMembers { get; set; }
        IPaymentRepository Payments { get; set; }
        IRegistrationRepository Registrations { get; set; }
    }
}
