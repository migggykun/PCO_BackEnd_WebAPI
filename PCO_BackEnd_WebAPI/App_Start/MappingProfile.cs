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
            Mapper.CreateMap<ApplicationUser, AccountsDTO>();
            Mapper.CreateMap<AccountsDTO, ApplicationUser>();
            Mapper.CreateMap<UserInfo, UserInfoDTO>();
            Mapper.CreateMap<UserInfoDTO, UserInfo>();
            Mapper.CreateMap<MembershipType, MembershipTypeDTO>();
            Mapper.CreateMap<MembershipTypeDTO, MembershipType>();
            Mapper.CreateMap<PRCDetail, PRCDetailDTO>();
            Mapper.CreateMap<PRCDetailDTO, PRCDetail>();
            Mapper.CreateMap<Conference, ConferenceDTO>();
            Mapper.CreateMap<ConferenceDTO, Conference>();
            Mapper.CreateMap<Rate, RateDTO>();
            Mapper.CreateMap<RateDTO, Rate>();
            Mapper.CreateMap<Promo, PromoDTO>();
            Mapper.CreateMap<PromoDTO, Promo>();
            Mapper.CreateMap<PromoMember, PromoMemberDTO>();
            Mapper.CreateMap<PromoMemberDTO, PromoMember>();
            

        }
    }
}