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
        public static PageResult<ResponsePaymentDTO> MapToPagedResponsePaymentDTO(PageResult<Payment> payments, IEnumerable<Conference> conferences, IEnumerable<UserInfo> userInfos)
        {
            string pType = "registration";
            var resultDTO = PaginationMapper<Payment, ResponsePaymentDTO>.MapResult(payments);
            foreach (var r in payments.Results)
            {
                if (string.Compare(r.paymentType, pType, true) == 0)
                {
                    int index = resultDTO.Results.ToList().FindIndex(x => x.RegistrationId == r.RegistrationId);
                    var userInfo = userInfos.First(x => x.Id == r.Registration.UserId);
                    var conference = conferences.First(x => x.Id == r.Registration.ConferenceId);
                    var payment = resultDTO.Results[index];
                    resultDTO.Results[index] = MapToResponsePaymentDTO(r, conference, userInfo, r.Registration.RegistrationStatusId);
                }
            }
            return resultDTO;
        }

        public static ResponsePaymentDTO MapToResponsePaymentDTO(Payment payment, Conference conference, UserInfo userInfo, int? registrationStatusId)
        {
            var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(payment);
            if (conference != null || userInfo != null)
            {
                resultDTO.Conference = ConferenceMapper.MapToResponseConferenceDTO(conference);
                resultDTO.UserInfo = Mapper.Map<UserInfo, ResponseUserInfoDTO>(userInfo);
                resultDTO.RegistrationStatusId = registrationStatusId;
            }
            return resultDTO;
        }
    }
}