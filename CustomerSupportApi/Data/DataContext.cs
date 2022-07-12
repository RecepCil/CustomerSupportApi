using CustomerSupportApi.Data.Entities;

namespace CustomerSupportApi.Data;

public class DataContext
{
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
}