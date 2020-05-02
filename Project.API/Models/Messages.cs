using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.API.Models
{
    [Serializable]
    public class Messages
    {
        [Key]
        public int MessageID { get; set; }

        public int SenderID { get; set; }

        public int ReceiverID { get; set; }

        public Users Sender { get; set; }

        public Users Receiver { get; set; }

        public string MessageContent { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadDate { get; set; }

        public DateTime SendDate { get; set; }

        public bool SenderDeleted { get; set; }

        public bool ReceiverDeleted { get; set; }





    }
}