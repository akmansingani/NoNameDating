using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.API.Models
{
    [Serializable]
    public class Likes
    {

        public int LikedUserID { get; set; }

        public int LikeByUserID { get; set; }

        public Users LikeByUser { get; set; }
        public Users LikedUser { get; set; }

    
       

    }
}