using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Dtos
{
    public class UserParamsDto
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int pagesize = 10;

        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int UserID { get; set; }

        public string Gender { get; set; }

        public int MinAge { get; set; } = 18;

        public int MaxAge { get; set; } = 99;

        public string Orderby { get; set; }

        public bool LikedUser { get; set; } = false;

        public bool LikedByUser { get; set; } = false;


    }
}
