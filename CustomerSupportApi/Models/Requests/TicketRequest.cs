using System.ComponentModel.DataAnnotations;
using CustomerSupportApi.Data.Enums;

namespace CustomerSupportApi.Models.Requests;

public class TicketRequest
{
    public TicketType Type { get; set; }

    public string CustomerEmail { get; set; }

    [Phone]
    public string CustomerPhone { get; set; }

    public string CustomerNumber { get; set; }

    public string Description { get; set; }
}