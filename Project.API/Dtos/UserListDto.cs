using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Dtos
{
    public class UserListDto
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string KnownAs { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ActiveDate { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string photourl { get; set; }

        public  ICollection<PhotoDto> Photos { get; set; }
    }
}
