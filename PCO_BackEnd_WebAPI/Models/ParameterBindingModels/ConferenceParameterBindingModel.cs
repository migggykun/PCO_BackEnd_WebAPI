using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.ParameterBindingModels
{
    public class ConferenceParameterBindingModel
    {
        public string Title { get; set; }

        public string Day { get; set; } 
        
        public string Month { get;set; } 
        
        public string Year { get;set; } 
        
        public string FromDate { get;set; }

        public string ToDate { get; set; }

        private int? _page { get; set; }
        public int? Page { get { return this._page == null ? 1 : this._page; } set { this._page = value; } }

        private int? _size { get; set; }
        public int? Size { get { return this._size == null ? 0 : this._size; } set{this._size = value;}}

    }
}