using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences
{
    public interface IRateRepository : IRepository<Rate>
    {
        List<Rate> AddRates(List<Rate> rates);
        Rate UpdateRate(int id, Rate rate);
        void RemoveRates(List<Rate> rates);
        
    }
}
