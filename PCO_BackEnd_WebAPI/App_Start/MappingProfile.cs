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

namespace PCO_BackEnd_WebAPI.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, ResponseAccountDTO>();
            Mapper.CreateMap<RequestAccountDTO, ApplicationUser>();
            Mapper.CreateMap<UserInfo, ResponseUserInfoDTO>();
            Mapper.CreateMap<RequestUserInfoDTO, UserInfo>();
            Mapper.CreateMap<MembershipType, ResponseMembershipTypeDTO>();
            Mapper.CreateMap<RequestMembershipTypeDTO, MembershipType>();
            Mapper.CreateMap<PRCDetail, ResponsePRCDetailDTO>();
            Mapper.CreateMap<RequestPRCDetailDTO, PRCDetail>();
            Mapper.CreateMap<AddConferenceDTO, Conference>();
            Mapper.CreateMap<UpdateConferenceDTO, Conference>();
            Mapper.CreateMap<Conference, ResponseConferenceDTO>();
            Mapper.CreateMap<AddRateWithConferenceDTO, Rate>();
            Mapper.CreateMap<UpdateRateWithConferenceDTO, Rate>();
            Mapper.CreateMap<RequestRateDTO, Rate>();
            Mapper.CreateMap<Rate, ResponseRateDTO>();
            Mapper.CreateMap<RequestPromoDTO, Promo>().ForMember(dst => dst.PromoMembers, src => src.MapFrom(p => p.MembershipTypeIds.Select(mId => new PromoMember { MembershipTypeId = mId}).ToList())); ;
            Mapper.CreateMap<Promo, ResponsePromoDTO>().ForMember(dst => dst.MembershipTypeIds, src => src.MapFrom(p => p.PromoMembers.Select(pm => pm.MembershipTypeId)));;
            Mapper.CreateMap<RequestPromoMemberDTO, PromoMember>();
            Mapper.CreateMap<PromoMember, ResponsePromoMemberDTO>();
            

        }
    }
}