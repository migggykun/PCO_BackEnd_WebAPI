using PCO_BackEnd_WebAPI.Models.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Bank
{
    interface IBankDetailRepository
    {
        BankDetail UpdateBankDetails(int id, BankDetail bankDetail);
    }
}
