using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SikayetKayit.Models.ViewModel
{
    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int FirstRowOnPage { get; set; }
        public int LastRowOnPage { get; set; }
    }
}