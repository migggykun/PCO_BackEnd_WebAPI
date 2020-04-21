using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Pagination
{
    public static class PaginationManager<TEntity> where TEntity : class
    {
        public static PageResult<TEntity> GetPagedResult(IQueryable<TEntity> query, int page, int size, int recordCount)
        {
            PageResult<TEntity> pageResult = new PageResult<TEntity>();
            int mod;
            int totalPageCount;
            int offset;
            int recordToReturn;
            if (size == 0)
            {
                mod = 0;
                totalPageCount = 1;
                offset = 0;
                recordToReturn = recordCount;
            }
            else
            {
                mod = recordCount % size;
                totalPageCount = (recordCount / size) + (mod == 0 ? 0 : 1);
                offset = size * (page - 1);
                recordToReturn = size;
            }
            pageResult.Results = query.OrderBy(GetPrimaryKeyExpression(typeof(TEntity)))
                                             .Skip(offset)
                                             .Take(recordToReturn)
                                             .ToList();
            pageResult.PageCount = totalPageCount;
            pageResult.RecordCount = recordCount;
            return pageResult;
        }

        private static Expression<Func<TEntity, int>> GetPrimaryKeyExpression(Type classType)
        {
            // Find primary key property based on primary key attribute.
            var keyProperty = classType.GetProperties().FirstOrDefault(p => (p.GetCustomAttributes(typeof(KeyAttribute), true).Any(two => two is KeyAttribute)) ||
                                                                             p.Name == "Id");

            // Create entity => portion of lambda expression
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "entity");

            // create entity.Id portion of lambda expression
            MemberExpression property = Expression.Property(parameter, keyProperty.Name);

            Expression<Func<TEntity, int>> predicate = Expression.Lambda<Func<TEntity, int>>(property, new[] { parameter });

            return predicate;
        }
    }
}