using System.ComponentModel.DataAnnotations;
using CustomerSupportApi.Data.Enums;

namespace CustomerSupportApi.Data.Entities;

public class Ticket
{
    public int Id { get; set; }

    public TicketStatus Status { get; set; }

    public TicketType Type { get; set; }

    [EmailAddress]
    public string CustomerEmail { get; set; }

    [Phone]
    public string CustomerPhone { get; set; }

    public string CustomerNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Description { get; set; }
}