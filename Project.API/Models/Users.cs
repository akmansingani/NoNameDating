using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Models
{
    [Serializable]
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Gender { get; set; }

        public Nullable<DateTime> DateofBirth { get; set; }

        public string KnownAs { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }

        public Nullable<DateTime> ActiveDate { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public virtual ICollection<Photos> Photos { get; set; }

        public virtual ICollection<Likes> LikeByUsers { get; set; }

        public virtual ICollection<Likes> LikedUsers { get; set; }

        public virtual ICollection<Messages> MessagesSent { get; set; }

        public virtual ICollection<Messages> MessagesReceived { get; set; }
    }
}
