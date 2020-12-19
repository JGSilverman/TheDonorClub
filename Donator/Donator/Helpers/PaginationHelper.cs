using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Helpers
{
    public class PaginationHelper
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public PaginationHelper(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
        }
    }
}
