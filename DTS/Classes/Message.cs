using System;

public class Message
{
	public int Id { get; set; }
	public int TicketId { get; set; }
	public string Author { get; set; }
	public DateTime SentAt { get; set; }
	public string Text { get; set; }
	public MessageType Type { get; set; }

    public enum MessageType
    {
        ForEmpoloyee,
        ForClient
    }
}
