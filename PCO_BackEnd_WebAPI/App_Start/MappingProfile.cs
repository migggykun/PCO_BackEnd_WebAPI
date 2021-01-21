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
using PCO_BackEnd_WebAPI.Models.Images;
using System.Globalization;
using PCO_BackEnd_WebAPI.DTOs.Activities;

namespace PCO_BackEnd_WebAPI.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Accounts
            Func<PRCDetail, ResponsePRCDetailDTO> convertToPRCDTO = (x) => x == null ? new ResponsePRCDetailDTO() { Id = string.Empty, IdNumber = string.Empty, ExpirationDate = string.Empty, RegistrationDate = string.Empty } :
                                                                             new ResponsePRCDetailDTO() { Id = x.Id.ToString(), IdNumber = x.IdNumber, ExpirationDate = x.ExpirationDate.Value.ToString("yyyy'-'MM'-'dd"), RegistrationDate = x.RegistrationDate.Value.ToString("yyyy'-'MM'-'dd") };
            Func <RequestPRCDetailDTO, PRCDetail> convertToPRCEntity = (x) =>
                                                                            {
                                                                                if (x != null && (string.IsNullOrEmpty(x.IdNumber) || string.IsNullOrEmpty(x.ExpirationDate) || string.IsNullOrEmpty(x.RegistrationDate)))
                                                                                {
                                                                                    if(string.IsNullOrEmpty(x.IdNumber))
                                                                                    {
                                                                                        return null;
                                                                                    }
                                                                                    else if (x != null && (!string.IsNullOrEmpty(x.IdNumber)))
                                                                                    {
                                                                                        return new PRCDetail()
                                                                                        {
                                                                                            IdNumber = x.IdNumber,
                                                                                            ExpirationDate = string.IsNullOrEmpty(x.ExpirationDate)?null: (DateTime?)DateTime.Parse(x.ExpirationDate),
                                                                                            RegistrationDate = string.IsNullOrEmpty(x.RegistrationDate) ? null : (DateTime?)DateTime.Parse(x.RegistrationDate)
                                                                                        };
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        return null;
                                                                                    }
                                                                                }
                                                                                else if (x != null && (!string.IsNullOrEmpty(x.IdNumber) || !string.IsNullOrEmpty(x.ExpirationDate) || !string.IsNullOrEmpty(x.RegistrationDate)))
                                                                                {
                                                                                    return new PRCDetail()
                                                                                    {
                                                                                        IdNumber = x.IdNumber,
                                                                                        ExpirationDate = DateTime.Parse(x.ExpirationDate),
                                                                                        RegistrationDate = DateTime.Parse(x.RegistrationDate)
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
            Mapper.CreateMap<Address, ResponseAddressDTO>();
            Mapper.CreateMap<RequestAddressDTO, Address>();
            Mapper.CreateMap<MembershipType, ResponseMembershipTypeDTO>();
            Mapper.CreateMap<RequestMembershipTypeDTO, MembershipType>();
            Mapper.CreateMap<RequestPRCDetailDTO, PRCDetail>().ConvertUsing(convertToPRCEntity);
            Mapper.CreateMap<PRCDetail, ResponsePRCDetailDTO>().ConvertUsing(convertToPRCDTO);
                                                        
            //Conference    
            Mapper.CreateMap<Conference, ResponseConferenceDTO>().ForMember(dst => dst.PromoId, src => src.MapFrom(c => (c.PromoId == null) ? string.Empty : c.PromoId.Value.ToString()))
                                                                 .ForMember(dst => dst.Rates, src => src.MapFrom(r => r.Rates.Where(x => x.conferenceActivityId == null).ToList()));
            Mapper.CreateMap<RequestPromoDTO, Promo>().ForMember(dst => dst.PromoMembers, src => src.MapFrom(p => p.MembershipTypeIds.Select(mId => new PromoMember { MembershipTypeId = mId }).ToList())); ;
            Mapper.CreateMap<Promo, ResponsePromoDTO>().ForMember(dst => dst.MembershipTypeIds, src => src.MapFrom(p => p.PromoMembers.Select(pm => pm.MembershipTypeId))); ;
            Mapper.CreateMap<AddConferenceDTO, Conference>().ForMember(dst => dst.Banner, src => src.Ignore());
            Mapper.CreateMap<UpdateConferenceDTO, Conference>().ForMember(dst => dst.Banner, src => src.Ignore());
            Mapper.CreateMap<AddRateWithConferenceDTO, Rate>();
            Mapper.CreateMap<UpdateRateWithConferenceDTO, Rate>();
            Mapper.CreateMap<RequestRateDTO, Rate>();
            Mapper.CreateMap<Rate, ResponseRateDTO>();
            Mapper.CreateMap<RequestPromoMemberDTO, PromoMember>();
            Mapper.CreateMap<PromoMember, ResponsePromoMemberDTO>();
            Mapper.CreateMap<RequestConferenceDayDTO, ConferenceDay>();
            Mapper.CreateMap<AddConferenceDayDTO, ConferenceDay>();
            Mapper.CreateMap<ConferenceDay, ResponseConferenceDayDTO>();
            Mapper.CreateMap<RequestConferenceActivityDTO, ConferenceActivity>();
            Mapper.CreateMap<AddConferenceActivityDTO, ConferenceActivity>();
            Mapper.CreateMap<ConferenceActivity, ResponseConferenceActivityDTO>();
            Mapper.CreateMap<RequestActivityScheduleDTO, ActivitySchedule>();
            Mapper.CreateMap<AddActivityScheduleDTO, ActivitySchedule>();
            Mapper.CreateMap<ActivitySchedule, ResponseActivityScheduleDTO>();
            Mapper.CreateMap<RequestActivityRateDTO, Rate>();
            Mapper.CreateMap<AddActivityRateDTO, Rate>();

            Mapper.CreateMap<Activity, ResponseActivityDTO>();
            Mapper.CreateMap<RequestActivityDTO, Activity>();
            Mapper.CreateMap<Member, ResponseMemberDTO>();
            Mapper.CreateMap<ResponseMemberDTO, Member>();
            Mapper.CreateMap<Member, RequestMemberDTO>();
            Mapper.CreateMap<RequestMemberDTO, Member>();

            //Registration
            Mapper.CreateMap<RequestRegistrationDTO, Registration>().ForMember(dst => dst.RegistrationStatusId, x => x.MapFrom(a => 1));
            Mapper.CreateMap<Registration, ResponseRegistrationDTO>();
            Mapper.CreateMap<Registration, ResponseListRegistrationDTO>();
            Mapper.CreateMap<RequestMemberRegistrationDTO, MemberRegistration>().ForMember(dst => dst.MemberRegistrationStatusId, x => x.MapFrom(a => 1));
            Mapper.CreateMap<MemberRegistration, ResponseMemberRegistrationDTO>();
            Mapper.CreateMap<MemberRegistration, ResponseListMemberRegistrationDTO>();
            Mapper.CreateMap<ApplicationUser, ResponseAccountDTO>();
            Mapper.CreateMap<AddPaymentDTO, Payment>().ForMember(dst => dst.Receipt, src => src.Ignore())
                                                      .ForMember(dst => dst.MemberRegistrationId, src => src.MapFrom(u => String.Compare(u.paymentType.Trim().ToLower(),"membership",true) == 0 ? (int?)u.refId : null))
                                                      .ForMember(dst => dst.RegistrationId, src => src.MapFrom(u => String.Compare(u.paymentType.Trim().ToLower(), "registration",true) == 0 ? (int?)u.refId : null));
            Mapper.CreateMap<UpdatePaymentDTO, Payment>().ForMember(dst => dst.Receipt, src => src.Ignore())
                                                      .ForMember(dst => dst.MemberRegistrationId, src => src.MapFrom(u => String.Compare(u.paymentType.Trim().ToLower(), "membership", true) == 0 ? (int?)u.refId : null))
                                                      .ForMember(dst => dst.RegistrationId, src => src.MapFrom(u => String.Compare(u.paymentType.Trim().ToLower(), "registration", true) == 0 ? (int?)u.refId : null));
            Mapper.CreateMap<Payment, ResponsePaymentDTO>().ForMember(dst => dst.refId, src => src.MapFrom(u => String.Compare(u.paymentType.Trim().ToLower(), "membership", true) == 0 ? (int)u.MemberRegistrationId : String.Compare(u.paymentType.Trim().ToLower(), "registration", true) == 0 ? (int)u.RegistrationId : 0));
            Mapper.CreateMap<RequestActivitiesToAttendDTO, ActivitiesToAttend>();
            Mapper.CreateMap<ActivitiesToAttend, ResponseActivitiesToAttendDTO>();

            //BankDetail
            Mapper.CreateMap<RequestAddBankDetailDTO, BankDetail>().ForMember(dst => dst.IsActive, src => src.MapFrom(p => true));
            Mapper.CreateMap<RequestUpdateBankDetailDTO, BankDetail>();
            Mapper.CreateMap<BankDetail, ResponseBankDetailDTO>();

        }
    }
}