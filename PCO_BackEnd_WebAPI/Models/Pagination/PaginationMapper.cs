using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
namespace PCO_BackEnd_WebAPI.Models.Pagination
{
    public static class PaginationMapper<TSource, TDestination> where TSource : class 
                                                                where TDestination : class
    {

        public static PageResult<TDestination> MapResult(PageResult<TSource> pg)
        {
            PageResult<TDestination>result = new PageResult<TDestination>()
            {
                RecordCount = pg.RecordCount,
                PageCount = pg.PageCount,
                Results = pg.Results.Select(Mapper.Map<TSource, TDestination>)
            };
            return result;
        }
    }
}