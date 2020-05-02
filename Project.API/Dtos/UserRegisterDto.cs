using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateofBirth { get; set; }

        [Required]
        public string KnownAs { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ActiveDate { get; set; }

        public UserRegisterDto()
        {
            CreatedDate = DateTime.Now;
            ActiveDate = DateTime.Now;
        }

    }
}
