using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Images.Helpers;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class PaymentMapper
    {
        public static PageResult<ResponsePaymentDTO> MapToPagedResponsePaymentDTO(PageResult<Payment> payments, IEnumerable<Conference> conferences, IEnumerable<UserInfo> userInfos, IEnumerable<MemberRegistration> memberRegistrations)
        {
            string pType = "registration";
            string mpType = "membership";
            var resultDTO = PaginationMapper<Payment, ResponsePaymentDTO>.MapResult(payments);
            foreach (var paymentResult in payments.Results)
            {
                if (string.Compare(paymentResult.paymentType, pType, true) == 0)
                {
                    int index = resultDTO.Results.ToList().FindIndex(x => x.RegistrationId == paymentResult.RegistrationId);
                    var userInfo = userInfos.First(x => x.Id == paymentResult.Registration.UserId);
                    Conference conference = null;
                    if (conferences == null)
                    {
                        continue;
                    }
                    foreach (Conference c in conferences)
                    {
                        if (c == null)
                        {
                            continue;
                        }
                        if (c.Id == paymentResult.Registration.ConferenceId)
                        {
                            conference = c;
                            break;
                        }
                    }
                    var payment = resultDTO.Results[index];
                    resultDTO.Results[index] = MapToResponsePaymentDTO(paymentResult, conference, userInfo, paymentResult.Registration.RegistrationStatusId);
                }
                else if (string.Compare(paymentResult.paymentType, mpType, true) == 0)
                {
                    //resultDTO.Results.Find(x => x.Id == paymentResult.Id); 
                    var userInfo = userInfos.First(x => x.Id == paymentResult.MemberRegistration.UserId);
                    MemberRegistration memberRegistration = null;
                    if (memberRegistrations == null)
                    {
                        continue;
                    }
                    foreach (MemberRegistration m in memberRegistrations)
                    {
                        if (m == null)
                        {
                            continue;
                        }
                        if (m.UserId == paymentResult.MemberRegistration.UserId)
                        {
                            memberRegistration = m;
                            break;
                        }
                    }
                    int index = -1;
                    int maxValidStatus = 3;
                    if (paymentResult.MemberRegistration.MemberRegistrationStatusId <= maxValidStatus)
                    {
                        index = resultDTO.Results.ToList().FindIndex(x => x.MemberRegistrationId == paymentResult.MemberRegistrationId && paymentResult.MemberRegistration.MemberRegistrationStatusId <= maxValidStatus);
                        var payment = resultDTO.Results[index];
                        resultDTO.Results[index] = MapToResponsePaymentDTO(paymentResult, null, userInfo, paymentResult.MemberRegistration.MemberRegistrationStatusId, memberRegistration != null ? (int?)memberRegistration.Id : null);
                    }
                    else
                    {
                        index = resultDTO.Results.ToList().FindIndex(x => x.MemberRegistrationId == paymentResult.MemberRegistrationId);
                        var payment = resultDTO.Results[index];
                        resultDTO.Results[index] = MapToResponsePaymentDTO(paymentResult, null, userInfo, paymentResult.MemberRegistration.MemberRegistrationStatusId, memberRegistration != null ? (int?)memberRegistration.Id : null);
                    }

                    
                }
                else
                {
                    //do nothing
                }
            }
            return resultDTO;
        }

        public static ResponsePaymentDTO MapToResponsePaymentDTO(Payment payment, Conference conference, UserInfo userInfo, int? registrationStatusId, int? memberRegistrationId=null)
        {
            var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(payment);
            if (conference != null || userInfo != null)
            {
                resultDTO.Conference = ConferenceMapper.MapToResponseConferenceDTO(conference);
                resultDTO.UserInfo = Mapper.Map<UserInfo, ResponseUserInfoDTO>(userInfo);
                resultDTO.RegistrationStatusId = registrationStatusId;
                resultDTO.MemberRegistrationId = memberRegistrationId;
            }
            return resultDTO;
        }
    }
}