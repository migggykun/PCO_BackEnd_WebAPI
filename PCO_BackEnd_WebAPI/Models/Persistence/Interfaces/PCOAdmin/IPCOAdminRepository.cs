using PCO_BackEnd_WebAPI.Models.PCOAdmin;
using PCO_BackEnd_WebAPI.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.PCOAdmin
{
    public interface IPCOAdminRepository
    {
        PCOAdminDetail UpdatePCOAdminDetails(float? price=null, string password=null);
        double GetAnnualFee();
        string GetPassword();
    }
}
