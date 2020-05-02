using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.API.Dtos
{
    public class MessageReturnDto
    {
        public int MessageID { get; set; }

        public int SenderID { get; set; }

        public int ReceiverID { get; set; }

        public string SenderKnownAs { get; set; }

        public string SenderUrl { get; set; }

        public string ReceiverKnownAs { get; set; }

        public string ReceiverUrl { get; set; }

        public string MessageContent { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadDate { get; set; }

        public DateTime SendDate { get; set; }

    }
}