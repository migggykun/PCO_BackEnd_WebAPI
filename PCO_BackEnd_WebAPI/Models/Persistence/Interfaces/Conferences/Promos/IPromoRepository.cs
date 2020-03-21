﻿using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences.Promos
{
    public interface IPromoRepository : IRepository<Promo>
    {
        PageResult<Promo> GetPagedPromos(int page, int size);
        Promo UpdatePromoDetails(int id, Promo promo);
    }
}
