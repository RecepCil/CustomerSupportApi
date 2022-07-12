using CustomerSupportApi.Data;
using CustomerSupportApi.Data.Entities;
using CustomerSupportApi.Data.Enums;
using CustomerSupportApi.Models.Requests;
using CustomerSupportApi.Models.Responses;

namespace CustomerSupportApi.Services;

    public class TicketService : ITicketService
    {
        private readonly DataContext _context;

        public TicketService(DataContext context)
        {
            _context = context;
        }

        public TicketResponse Get(int id)
        {
            Ticket? ticket = _context.Tickets.FirstOrDefault(item => item.Id == id);

            if(ticket == null)
            {
                return null;
            }

            return new TicketResponse()
            {
                Id = ticket.Id,
                CreatedAt = ticket.CreatedAt,
                CustomerEmail = ticket.CustomerEmail,
                CustomerNumber = ticket.CustomerNumber,
                CustomerPhone = ticket.CustomerPhone,
                Description = ticket.Description,
                Status = ticket.Status,
                Type = ticket.Type
            }; 
        }

        public List<TicketResponse> GetAll()
        {
            List<Ticket>? tickets = _context.Tickets?.ToList();

            if(tickets == null)
            {
                return new List<TicketResponse>();
            }

            return tickets
                .Select(i => new TicketResponse()
                {
                    CreatedAt = i.CreatedAt,
                    CustomerEmail = i.CustomerEmail,
                    CustomerNumber = i.CustomerNumber,
                    CustomerPhone = i.CustomerPhone,
                    Description = i.Description,
                    Id = i.Id,
                    Status = i.Status,
                    Type = i.Type
                    
                }).ToList();
        }

        public TicketResponse Add(TicketRequest request)
        {
            Ticket ticket = new Ticket()
            {
                CreatedAt = DateTime.UtcNow,
                CustomerEmail = request.CustomerEmail,
                CustomerNumber = request.CustomerNumber,
                CustomerPhone = request.CustomerPhone,
                Description = request.Description,
                Id = GenerateNextId(),
                Status = TicketStatus.Created,
                Type = request.Type
            };

            _context.Tickets.Add(ticket);

            return new TicketResponse()
            {
                Id = ticket.Id,
                CreatedAt = ticket.CreatedAt,
                CustomerEmail = ticket.CustomerEmail,
                CustomerNumber = ticket.CustomerNumber,
                CustomerPhone = ticket.CustomerPhone,
                Description = ticket.Description,
                Status = ticket.Status,
                Type = ticket.Type
            };
        }

        public TicketResponse Update(int id, TicketRequest request)
        {
            Ticket? ticket = GetById(id);

            if (ticket == null)
            {
                return null;
            }

            int index = _context.Tickets.FindIndex(item => item.Id == id);

            ticket.CustomerEmail = request.CustomerEmail;
            ticket.CustomerNumber = request.CustomerNumber;
            ticket.CustomerPhone = request.CustomerPhone;
            ticket.Description = request.Description;
            ticket.Type = request.Type;

            _context.Tickets[index] = ticket;

            return new TicketResponse()
            {
                Id = ticket.Id,
                CreatedAt = ticket.CreatedAt,
                CustomerEmail = ticket.CustomerEmail,
                CustomerNumber = ticket.CustomerNumber,
                CustomerPhone = ticket.CustomerPhone,
                Description = ticket.Description,
                Status = ticket.Status,
                Type = ticket.Type
            };
        }

        public void Delete(int id)
        {
            Ticket? ticket = GetById(id);

            if (ticket == null)
            {
                return;
            }

            _context.Tickets.Remove(ticket);
        }

        private Ticket GetById(int id)
        {
            return _context.Tickets?.FirstOrDefault(x => x.Id == id);
        }

        private int GenerateNextId()
        {
            List<Ticket> tickets = _context.Tickets;
            
            int id = tickets.Any() ? tickets.Max(i => i.Id) : 0;

            return ++id;
        }
    }
