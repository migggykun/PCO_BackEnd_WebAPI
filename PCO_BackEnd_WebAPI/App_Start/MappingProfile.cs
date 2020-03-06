using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.AccountBindingModels;
using PCO_BackEnd_WebAPI.DTOs.Accounts;

namespace PCO_BackEnd_WebAPI.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, AccountsDTO>();
            Mapper.CreateMap<AccountsDTO, ApplicationUser>();
            Mapper.CreateMap<PRCDetail, PRCDetailDTO>();
            Mapper.CreateMap<PRCDetailDTO, PRCDetail>();
            Mapper.CreateMap<UserInfo, UserInfoDTO>();
            Mapper.CreateMap<UserInfoDTO, UserInfo>();
            Mapper.CreateMap<MembershipAssignment, MembershipAssignmentDTO>();
            Mapper.CreateMap<MembershipAssignmentDTO, MembershipAssignment>();
            Mapper.CreateMap<MembershipType, MembershipTypeDTO>();
            Mapper.CreateMap<MembershipTypeDTO, MembershipType>();
            

        }
    }
}