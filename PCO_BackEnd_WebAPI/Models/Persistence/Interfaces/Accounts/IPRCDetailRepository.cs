﻿using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts
{
    public interface IPRCDetailRepository : IRepository<PRCDetail>
    {
        PageResult<PRCDetail> GetPagedPRCDetail(int page, 
                                                int size, 
                                                string filter,
                                                DateTime? ExpirationDateFrom,
                                                DateTime? ExpirationDateTo);
        PRCDetail GetPRCDetailById(string prcId);
        PRCDetail Update(int id, PRCDetail entityToUpdate);
    }
}
