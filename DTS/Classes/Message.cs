using System;

namespace DTS
{
    public class Message
    {
        public int Id { get; set; }
        public int TicketId { get; set; }

        public AuthorType AuthorType { get; set; }
        public int AuthorId { get; set; }

        public DateTime SentAt { get; set; }
        public string Text { get; set; }
    }

    public enum AuthorType
    {
        Client,
        Employee
    }
}

