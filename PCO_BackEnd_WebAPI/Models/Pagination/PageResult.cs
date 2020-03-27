using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Pagination
{
    public class PageResult<TEntity> where TEntity : class
    {
        /// <summary> 
        /// The total number of pages available. 
        /// </summary> 
        [Required]
        public int PageCount { get; set; }

        /// <summary> 
        /// The total number of records available. 
        /// </summary> 
        [Required]
        public int RecordCount { get; set; }

        /// <summary> 
        /// The records this page represents. 
        /// </summary> 
        [Required]
        public List<TEntity> Results { get; set; }
    }
}