using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Helpers
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }


        public PageList(List<T> items, int count, int pageno, int pagesize)
        {
            TotalCount = count;
            CurrentPage = pageno;
            PageSize = pagesize;
            TotalPages = (int)Math.Ceiling(count / (double)pagesize);
            this.AddRange(items);
        }

        public static async Task<PageList<T>> CreateAsync(int pageno, int pagesize, IQueryable<T> source)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageno - 1) * pagesize).Take(pagesize).ToListAsync();

            return new PageList<T>(items, count, pageno, pagesize);

        }

    }
}
