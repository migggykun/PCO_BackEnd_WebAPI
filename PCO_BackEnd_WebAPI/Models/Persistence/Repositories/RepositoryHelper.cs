using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories
{
    public static class RepositoryHelper<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets the primary Key of specified entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int GetPrimaryKey(DbContext context, TEntity entity)
        {
            int primaryKey;
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            ObjectSet<TEntity> set = objectContext.CreateObjectSet<TEntity>();
            string primaryKeyName = set.EntitySet.ElementType
                                                        .KeyMembers
                                                        .Select(k => k.Name)
                                                        .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(primaryKeyName))
            {
                primaryKey = (int)context.Entry<TEntity>(entity).Property(primaryKeyName).CurrentValue;
            }
            else
            {
                primaryKey = 0; //entity does not exist in context
            }
            return primaryKey;
        }
    }
}