using CustomerSupportApi.Models.Requests;
using CustomerSupportApi.Models.Responses;

namespace CustomerSupportApi.Services;

public interface ITicketService
{
    TicketResponse Get(int id);

    List<TicketResponse> GetAll();

    TicketResponse Add(TicketRequest ticketRequest);

    TicketResponse Update(int id, TicketRequest ticketRequest);

    void Delete(int id);
}