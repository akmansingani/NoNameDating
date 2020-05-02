using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Dtos
{
    public class UserUpdateDto
    {
        public int UserID { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
      
    }
}
