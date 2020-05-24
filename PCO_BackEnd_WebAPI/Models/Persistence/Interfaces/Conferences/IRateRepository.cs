using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences
{
    public interface IRateRepository : IRepository<Rate>
    {
        PageResult<Rate> GetPagedRates(int page, int size);
        List<Rate> AddRates(List<Rate> rates);
        Rate UpdateRate(int id, Rate rate);
        void RemoveRates(List<Rate> rates);
        Rate GetRate(Expression<Func<Rate, bool>> predicate);
        
    }
}
