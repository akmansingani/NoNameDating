using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Dtos
{
    public class PhotoDto
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public bool IsMain { get; set; }

        public DateTime CreatedDate { get; set; }

        public IFormFile file { get; set; }

        public string PublicID { get; set; }
    }
}
