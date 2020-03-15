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
            Mapper.CreateMap<Conference, ConferenceDTO>();
            Mapper.CreateMap<ConferenceDTO, Conference>();
            Mapper.CreateMap<Rate, RateDTO>();
            Mapper.CreateMap<RateDTO, Rate>();
            Mapper.CreateMap<Promo, PromoDTO>();
            Mapper.CreateMap<PromoDTO, Promo>();
            Mapper.CreateMap<PromoMember, PromoMemberDTO>();
            Mapper.CreateMap<PromoMemberDTO, PromoMember>();
            Mapper.CreateMap<ConferenceRegistrationDTO, Registration>();
            Mapper.CreateMap<Registration, ConferenceRegistrationDTO>();
        }
    }
}