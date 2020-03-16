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
            Mapper.CreateMap<PRCDetail, ResponsePRCDetailDTO>().ForMember(dst => dst.ExpirationDate, src => src.MapFrom(prc => prc.ExpirationDate.ToString()))
                                                               .ForMember(dst => dst.Id, src => src.MapFrom(prc => prc.Id.ToString()));
                                                               
            Mapper.CreateMap<RequestPRCDetailDTO, PRCDetail>().ForMember(dst => dst.ExpirationDate, src => src.MapFrom(prc => DateTime.Parse(prc.ExpirationDate)));
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
            Mapper.CreateMap<RequestRegistrationDTO, Registration>();
            Mapper.CreateMap<Registration, ResponseRegistrationDTO>().ForMember(dst => dst.RegistrationDate, src => src.MapFrom(r => r.RegistrationDate == null ? string.Empty : r.RegistrationDate.ToString()));
            Mapper.CreateMap<ApplicationUser, ResponseAccountDTO>().ForMember(dst => dst.PRCDetail, src => src.MapFrom(p => p.PRCDetail == null ? new ResponsePRCDetailDTO()
                                                                                                                                    {
                                                                                                                                        Id = string.Empty,
                                                                                                                                        IdNumber = string.Empty,
                                                                                                                                        ExpirationDate = string.Empty
                                                                                                                                    } :
                                                                                                                                      new ResponsePRCDetailDTO()
                                                                                                                                      {
                                                                                                                                          Id = p.PRCDetail.Id.ToString(),
                                                                                                                                          IdNumber = p.PRCDetail.IdNumber,
                                                                                                                                          ExpirationDate = p.PRCDetail.ExpirationDate.ToShortDateString()
                                                                                                                                      }));

        }
    }
}