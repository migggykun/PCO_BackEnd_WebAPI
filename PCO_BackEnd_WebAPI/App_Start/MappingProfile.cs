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

namespace PCO_BackEnd_WebAPI.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<RequestAccountDTO, ApplicationUser>();
            Mapper.CreateMap<UserInfo, ResponseUserInfoDTO>();
            Mapper.CreateMap<RequestUserInfoDTO, UserInfo>();
            Mapper.CreateMap<MembershipType, ResponseMembershipTypeDTO>();
            Mapper.CreateMap<RequestMembershipTypeDTO, MembershipType>();

            Func<PRCDetail, ResponsePRCDetailDTO> convDTO = (x) => x == null ? new ResponsePRCDetailDTO() { Id = string.Empty, IdNumber = string.Empty, ExpirationDate = string.Empty } :
                                                                               new ResponsePRCDetailDTO() { Id = x.Id.ToString(), IdNumber = x.IdNumber, ExpirationDate = x.ExpirationDate.Date.ToShortDateString() };
            Mapper.CreateMap<PRCDetail, ResponsePRCDetailDTO>().ConvertUsing(convDTO);

            Func<RequestPRCDetailDTO, PRCDetail> conv = (x) => x.IdNumber == string.Empty ? null : new PRCDetail() { IdNumber = x.IdNumber, ExpirationDate = DateTime.Parse(x.ExpirationDate) };
            Mapper.CreateMap<RequestPRCDetailDTO, PRCDetail>().ConvertUsing(conv);
                                                                        
            Mapper.CreateMap<AddConferenceDTO, Conference>();
            Mapper.CreateMap<UpdateConferenceDTO, Conference>();
            Mapper.CreateMap<Conference, ResponseConferenceDTO>().ForMember(dst => dst.PromoId, src => src.MapFrom(c => (c.PromoId == null)?string.Empty : c.PromoId.Value.ToString()))
                                                                 .ForMember(dst => dst.Banner, src => src.MapFrom(c => string.IsNullOrEmpty(c.Banner) ? string.Empty : c.Banner));
            Mapper.CreateMap<AddRateWithConferenceDTO, Rate>();
            Mapper.CreateMap<UpdateRateWithConferenceDTO, Rate>();
            Mapper.CreateMap<RequestRateDTO, Rate>();
            Mapper.CreateMap<Rate, ResponseRateDTO>();
            Mapper.CreateMap<RequestPromoDTO, Promo>().ForMember(dst => dst.PromoMembers, src => src.MapFrom(p => p.MembershipTypeIds.Select(mId => new PromoMember { MembershipTypeId = mId}).ToList())); ;
            Mapper.CreateMap<Promo, ResponsePromoDTO>().ForMember(dst => dst.MembershipTypeIds, src => src.MapFrom(p => p.PromoMembers.Select(pm => pm.MembershipTypeId)));;
            Mapper.CreateMap<RequestPromoMemberDTO, PromoMember>();
            Mapper.CreateMap<PromoMember, ResponsePromoMemberDTO>();
            Mapper.CreateMap<RequestRegistrationDTO, Registration>().ForMember(dst => dst.RegistrationStatusId, x => x.MapFrom(a => 1));
            Mapper.CreateMap<Registration, ResponseRegistrationDTO>();
            Mapper.CreateMap<Registration, ResponseListRegistrationDTO>();
            Mapper.CreateMap<ApplicationUser, ResponseAccountDTO>();

            Mapper.CreateMap<AddPaymentDTO, Payment>();
            Mapper.CreateMap<UpdatePaymentDTO, Payment>();
            Mapper.CreateMap<Payment, ResponsePaymentDTO>();

        }
    }
}