using Microsoft.AspNetCore.Http;
using OnlineStore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Options
{
    public static class Extensions
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            response.Headers.Add("Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
            // CORS
            response.Headers.Add("access-control-expose-headers", "Pagination");
        }
    }
}
