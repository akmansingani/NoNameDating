using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.API.Models
{
    [Serializable]
    public class Photos
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public bool IsMain { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public Users user { get; set; }

        public string PublicID { get; set; }

    }
}