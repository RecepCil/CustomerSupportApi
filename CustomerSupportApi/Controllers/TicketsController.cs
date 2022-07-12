using CustomerSupportApi.Models.Requests;
using CustomerSupportApi.Models.Responses;
using CustomerSupportApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupportApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // GET: api/Tickets
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TicketResponse>))]
    public async Task<ActionResult> GetTickets()
    {
        List<TicketResponse>? response = _ticketService.GetAll();

        return Ok(response);
    }

    // GET: api/Tickets/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketResponse>> GetTicket(int id)
    {
        TicketResponse? response = _ticketService.Get(id);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    // PUT: api/Tickets/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutTicket(int id, TicketRequest ticket)
    {
        TicketResponse? ticketToUpdate = _ticketService.Update(id, ticket);

        if (ticketToUpdate == null)
        {
            return NotFound();
        }

        return Ok(ticketToUpdate);
    }

    // POST: api/Tickets
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TicketResponse))]
    public async Task<ActionResult<TicketResponse>> PostTicket(TicketRequest ticket)
    {
        var response = _ticketService.Add(ticket);

        return Created($"api/Tickets/{response.Id}", response);
    }

    // DELETE: api/Tickets/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        _ticketService.Delete(id);

        return NoContent();
    }
}