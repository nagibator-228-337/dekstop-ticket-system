using System;

public class Ticket
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public TicketStatus Status { get; set; }
    public Employee? AssignedEmployee { get; set; }
    public string AccessCode { get; set; }
    public string? ClientContact { get; set; }

    public enum TicketStatus
    {
        New,
        InProgress,
        Done
    }
}
