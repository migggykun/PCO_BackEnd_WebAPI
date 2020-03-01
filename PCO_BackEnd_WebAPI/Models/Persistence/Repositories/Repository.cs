using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;


namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        private DbSet<TEntity> _entity;

        public Repository(DbContext Context)
        {
            _context = Context;
            _entity = _context.Set<TEntity>();
        }

        /// <summary>
        /// Get object based on provided Id
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns> Singl object</returns>
        public TEntity Get(int id)
        {
            return _entity.Find(id) as TEntity;
        }

        /// <summary>
        /// Get entity data.
        /// </summary>
        /// <returns>All data of table</returns>
        public IEnumerable<TEntity> GetAll()
        {
            return _entity.ToList();
        }

        /// <summary>
        /// Queries list of object based on given lambda expression.
        /// </summary>
        /// <param name="predicate">Lambda expression</param>
        /// <returns>Returns list of queried objects</returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entity.Where(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            _entity.Add(entity);
        }

        /// <summary>
        /// Add list of data in database.
        /// </summary>
        /// <param name="entities">Objects to be added in database</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entity.AddRange(entities);
        }

        /// <summary>
        /// Remove the object in database
        /// </summary>
        /// <param name="entity">Object to be removed</param>
        public void Remove(TEntity entity)
        {
            _entity.Remove(entity);
        }

        /// <summary>
        /// Updates Object
        /// </summary>
        /// <param name="oldEntity">Old data</param>
        /// <param name="newEntity">New data</param>
        public void Update(TEntity newEntity)
        {
            int primaryKey = RepositoryHelper<TEntity>.GetPrimaryKey(_context, newEntity); //Acquires the primary key of generic object
            var oldEntity = _entity.Find(primaryKey); //Acquires original object

            foreach (PropertyInfo p in newEntity.GetType().GetProperties())
            {
                var propertyValue = p.GetValue(newEntity, null); //acquires value of iterated property
                if (propertyValue != null)
                {
                    _context.Entry<TEntity>(oldEntity).Property(p.Name).CurrentValue = propertyValue;
                }
            }
        }


    }
}