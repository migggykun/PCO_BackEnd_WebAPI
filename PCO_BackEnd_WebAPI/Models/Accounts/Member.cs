using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Accounts
{
    [Table("pc0_Database_Staging.[dbo.Members]")]
    public partial class Member
    {
        public Member()
        {
        }

        public Member(int userId)
        {
            UserId = userId;
            IsActive = true;
            MemberSince = DateTime.Now;
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime MemberSince { get; set; }
    }
}