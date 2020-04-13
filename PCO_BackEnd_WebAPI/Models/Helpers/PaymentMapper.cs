using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences;
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
            var resultDTO = PaginationMapper<Payment, ResponsePaymentDTO>.MapResult(payments);
            foreach (var r in payments.Results)
            {
                int index = resultDTO.Results.ToList().FindIndex(x => x.RegistrationId == r.RegistrationId);
                var userInfo = userInfos.First(x => x.Id == r.Registration.UserId);
                var conference = conferences.First(x => x.Id == r.Registration.ConferenceId);
                var payment = resultDTO.Results[index];
                resultDTO.Results[index] = MapToResponsePaymentDTO(r, conference, userInfo);
            }
            return resultDTO;
        }

        public static ResponsePaymentDTO MapToResponsePaymentDTO(Payment payment, Conference conference, UserInfo userInfo)
        {
            var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(payment);
            resultDTO.Conference = ConferenceMapper.MapToResponseConferenceDTO(conference);
            resultDTO.UserInfo = Mapper.Map<UserInfo, ResponseUserInfoDTO>(userInfo);
            resultDTO.ProofOfPayment = Convert.ToBase64String(payment.ProofOfPayment);
            return resultDTO;
        }
    }
}