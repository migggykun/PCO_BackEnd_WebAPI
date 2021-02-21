using PCO_BackEnd_WebAPI.Models.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Accounts
{
    [Table("Members")]
    public partial class Member
    {
        public Member()
        {
        }

        public Member(int userId)
        {
            UserId = userId;
            IsActive = true;
            MemberSince = PhTime.Now();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime MemberSince { get; set; }
    }
}