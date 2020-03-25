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
        public static void MapToResponsePaymentDTO(ResponsePaymentDTO payment, UserInfo userInfo, Conference conference)
        {
                var userInfoDTO = Mapper.Map<UserInfo, ResponseUserInfoDTO>(userInfo);
                var conferenceDTO = Mapper.Map<Conference, ResponseConferenceDTO>(conference);
                payment.UserInfo = userInfoDTO;
                payment.Conference = conferenceDTO;
        }
    }
}