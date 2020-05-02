using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.API.Dtos
{
    public class MessageCreateDto
    {
        public int MessageID { get; set; }

        public int SenderID { get; set; }

        public int ReceiverID { get; set; }

        public string MessageContent { get; set; }

        public DateTime SendDate { get; set; }

        public MessageCreateDto()
        {
            SendDate = DateTime.Now;
        }

    }
}