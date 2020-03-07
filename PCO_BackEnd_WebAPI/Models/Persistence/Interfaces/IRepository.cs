using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //Query Data
        //GET Operation
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        //ADD Operation
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        //DELETE Operation
        void Remove(TEntity entity);

        //UPDATE Operation
        void Update(TEntity newEntity);
    }
}
