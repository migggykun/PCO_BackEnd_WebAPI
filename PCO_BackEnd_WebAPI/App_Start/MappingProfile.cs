using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.AccountBindingModels;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.DTOs.Conferences.Promos;
using PCO_BackEnd_WebAPI.DTOs;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
using PCO_BackEnd_WebAPI.DTOs.Bank;
using PCO_BackEnd_WebAPI.Models.Bank;

namespace PCO_BackEnd_WebAPI.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Accounts
            Func<PRCDetail, ResponsePRCDetailDTO> convDTO = (x) => x == null ? new ResponsePRCDetailDTO() { Id = string.Empty, IdNumber = string.Empty, ExpirationDate = string.Empty } :
                                                                             new ResponsePRCDetailDTO() { Id = x.Id.ToString(), IdNumber = x.IdNumber, ExpirationDate = x.ExpirationDate.Date.ToShortDateString() };
            Func<RequestPRCDetailDTO, PRCDetail> conv = (x) =>
                                                            {
                                                                if (x != null && (string.IsNullOrEmpty(x.IdNumber) || string.IsNullOrEmpty(x.ExpirationDate)))
                                                                {
                                                                    return null;
                                                                }
                                                                else if (x != null && (!string.IsNullOrEmpty(x.IdNumber) || !string.IsNullOrEmpty(x.ExpirationDate)))
                                                                {
                                                                    return new PRCDetail()
                                                                    {
                                                                        IdNumber = x.IdNumber,
                                                                        ExpirationDate = DateTime.Parse(x.ExpirationDate)

                                                                    };
                                                                }
                                                                else //RequestPRCDetailDTO is null
                                                                {
                                                                    return null;
                                                                }
                                                            };


            Mapper.CreateMap<RequestAccountDTO, ApplicationUser>();
            Mapper.CreateMap<UserInfo, ResponseUserInfoDTO>();
            Mapper.CreateMap<RequestUserInfoDTO, UserInfo>();
            Mapper.CreateMap<MembershipType, ResponseMembershipTypeDTO>();
            Mapper.CreateMap<RequestMembershipTypeDTO, MembershipType>();
            Mapper.CreateMap<RequestPRCDetailDTO, PRCDetail>().ConvertUsing(conv);
            Mapper.CreateMap<PRCDetail, ResponsePRCDetailDTO>().ConvertUsing(convDTO);
                                                        
            //Conference    
            Mapper.CreateMap<Conference, ResponseConferenceDTO>().ForMember(dst => dst.PromoId, src => src.MapFrom(c => (c.PromoId == null) ? string.Empty : c.PromoId.Value.ToString()));
            Mapper.CreateMap<RequestPromoDTO, Promo>().ForMember(dst => dst.PromoMembers, src => src.MapFrom(p => p.MembershipTypeIds.Select(mId => new PromoMember { MembershipTypeId = mId }).ToList())); ;
            Mapper.CreateMap<Promo, ResponsePromoDTO>().ForMember(dst => dst.MembershipTypeIds, src => src.MapFrom(p => p.PromoMembers.Select(pm => pm.MembershipTypeId))); ;
            Mapper.CreateMap<AddConferenceDTO, Conference>().ForMember(dst => dst.Banner, src => src.MapFrom(c => (c.Banner == null) ? Enumerable.Empty<byte>().ToArray() : new byte[]{}));
            Mapper.CreateMap<UpdateConferenceDTO, Conference>().ForMember(dst => dst.Banner, src => src.MapFrom(c => (c.Banner == null) ? Enumerable.Empty<byte>().ToArray() : new byte[] {})); ;
            Mapper.CreateMap<AddRateWithConferenceDTO, Rate>();
            Mapper.CreateMap<UpdateRateWithConferenceDTO, Rate>();
            Mapper.CreateMap<RequestRateDTO, Rate>();
            Mapper.CreateMap<Rate, ResponseRateDTO>();
            Mapper.CreateMap<RequestPromoMemberDTO, PromoMember>();
            Mapper.CreateMap<PromoMember, ResponsePromoMemberDTO>();

            //Registration
            Mapper.CreateMap<RequestRegistrationDTO, Registration>().ForMember(dst => dst.RegistrationStatusId, x => x.MapFrom(a => 1));
            Mapper.CreateMap<Registration, ResponseRegistrationDTO>();
            Mapper.CreateMap<Registration, ResponseListRegistrationDTO>();
            Mapper.CreateMap<ApplicationUser, ResponseAccountDTO>();
            Mapper.CreateMap<AddPaymentDTO, Payment>().ForMember(dst => dst.ProofOfPayment, src => src.MapFrom(x => new byte[]{}));
            Mapper.CreateMap<UpdatePaymentDTO, Payment>().ForMember(dst => dst.ProofOfPayment, src => src.MapFrom(x => new byte[]{}));
            Mapper.CreateMap<Payment, ResponsePaymentDTO>();
            Mapper.CreateMap<RequestBankDetailDTO, BankDetail>();
            Mapper.CreateMap<BankDetail, ResponseBankDetailDTO>();

        }
    }
}