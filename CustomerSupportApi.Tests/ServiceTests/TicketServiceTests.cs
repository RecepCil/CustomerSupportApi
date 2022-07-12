using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using CustomerSupportApi.Data;
using CustomerSupportApi.Data.Entities;
using CustomerSupportApi.Data.Enums;
using CustomerSupportApi.Models.Requests;
using CustomerSupportApi.Models.Responses;
using CustomerSupportApi.Services;
using FluentAssertions;
using Xunit;

namespace CustomerSupportApi.Tests.ServiceTests;

public class TicketServiceTests
{
    [Fact]
    public void Get_FilteredTicketExists_ReturnCorrectTicket()
    {
        // Arrange
        DataContext context = new DataContext();

        int ticketId = 1;

        Ticket ticket = new Faker<Ticket>()
            .RuleFor(t => t.Id, ticketId)
            .RuleFor(t => t.CreatedAt, DateTime.UtcNow)
            .RuleFor(t => t.CustomerEmail, f => f.Internet.ExampleEmail())
            .RuleFor(t => t.CustomerNumber, f => f.Random.Number().ToString())
            .RuleFor(t => t.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(t => t.Description, Guid.NewGuid().ToString())
            .RuleFor(t => t.Status, f => f.PickRandom<TicketStatus>())
            .RuleFor(t => t.Type, f => f.PickRandom<TicketType>());

        context.Tickets.Add(ticket);

        TicketService sut = new TicketService(context);

        // Act
        TicketResponse? response = sut.Get(ticketId);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(ticketId);
        response.Description.Should().Be(ticket.Description);
        response.CustomerEmail.Should().Be(ticket.CustomerEmail);
    }

    [Fact]
    public void Get_FilteredTicketNotExists_ReturnNull()
    {
        // Arrange
        DataContext context = new DataContext();

        int ticketId = 1;

        TicketService sut = new TicketService(context);

        // Act
        TicketResponse? response = sut.Get(ticketId);

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public void GetAll_TicketsExist_ReturnTickets()
    {
        // Arrange
        DataContext context = new DataContext();

        List<Ticket> tickets = new Faker<Ticket>()
            .RuleFor(t => t.Id, f => f.Random.Int())
            .RuleFor(t => t.CreatedAt, DateTime.UtcNow)
            .RuleFor(t => t.CustomerEmail, f => f.Internet.ExampleEmail())
            .RuleFor(t => t.CustomerNumber, f => f.Random.Number().ToString())
            .RuleFor(t => t.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(t => t.Description, Guid.NewGuid().ToString())
            .RuleFor(t => t.Status, f => f.PickRandom<TicketStatus>())
            .RuleFor(t => t.Type, f => f.PickRandom<TicketType>())
            .Generate(2);

        context.Tickets.AddRange(tickets);

        TicketService sut = new TicketService(context);

        // Act
        List<TicketResponse> response = sut.GetAll();

        // Assert
        response.Should().NotBeNull();
        response.Count.Should().Be(tickets.Count);
        response.First().Id.Should().Be(tickets.First().Id);
        response.First().CreatedAt.Should().Be(tickets.First().CreatedAt);
        response.First().CustomerEmail.Should().Be(tickets.First().CustomerEmail);
        response.First().CustomerNumber.Should().Be(tickets.First().CustomerNumber);
        response.First().CustomerPhone.Should().Be(tickets.First().CustomerPhone);
        response.First().Description.Should().Be(tickets.First().Description);
        response.First().Status.Should().Be(tickets.First().Status);
        response.First().Type.Should().Be(tickets.First().Type);
    }

    [Fact]
    public void GetAll_TicketsNotExist_ReturnEmptyList()
    {
        // Arrange
        DataContext context = new DataContext();

        TicketService sut = new TicketService(context);

        // Act
        List<TicketResponse> response = sut.GetAll();

        // Assert
        response.Should().BeEmpty();
        response.Count.Should().Be(0);
    }

    [Fact]
    public void Add_AppropriateTicket_ReturnAddedTicket()
    {
        // Arrange
        DataContext context = new DataContext();

        TicketRequest ticket = new Faker<TicketRequest>()
            .RuleFor(t => t.CustomerEmail, f => f.Internet.ExampleEmail())
            .RuleFor(t => t.CustomerNumber, f => f.Random.Number().ToString())
            .RuleFor(t => t.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(t => t.Description, Guid.NewGuid().ToString())
            .RuleFor(t => t.Type, f => f.PickRandom<TicketType>());

        TicketService sut = new TicketService(context);

        // Act
        TicketResponse response = sut.Add(ticket);

        // Assert
        response.Should().NotBeNull();

        Ticket expectedTicket = context.Tickets.First();

        response.CustomerEmail.Should().Be(expectedTicket.CustomerEmail);
        response.CustomerNumber.Should().Be(expectedTicket.CustomerNumber);
        response.CustomerPhone.Should().Be(expectedTicket.CustomerPhone);
        response.Description.Should().Be(expectedTicket.Description);
        response.Status.Should().Be(expectedTicket.Status);
        response.Type.Should().Be(expectedTicket.Type);
    }

    [Fact]
    public void Update_NotExistTicket_ReturnNull()
    {
        // Arrange
        DataContext context = new DataContext();

        int ticketId = 1;

        TicketService sut = new TicketService(context);

        // Act
        TicketResponse? response = sut.Update(ticketId, new TicketRequest());

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public void Update_ExistTicket_ReturnUpdatedTicket()
    {
        // Arrange
        DataContext context = new DataContext();

        int ticketId = 1;

        Ticket ticket = new Faker<Ticket>()
            .RuleFor(t => t.Id, ticketId)
            .RuleFor(t => t.CreatedAt, DateTime.UtcNow)
            .RuleFor(t => t.CustomerEmail, f => f.Internet.ExampleEmail())
            .RuleFor(t => t.CustomerNumber, f => f.Random.Number().ToString())
            .RuleFor(t => t.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(t => t.Description, Guid.NewGuid().ToString())
            .RuleFor(t => t.Status, f => f.PickRandom<TicketStatus>())
            .RuleFor(t => t.Type, f => f.PickRandom<TicketType>());

        context.Tickets.Add(ticket);

        TicketRequest updatedTicket = new Faker<TicketRequest>()
            .RuleFor(t => t.CustomerEmail, f => f.Internet.ExampleEmail())
            .RuleFor(t => t.CustomerNumber, f => f.Random.Number().ToString())
            .RuleFor(t => t.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(t => t.Description, Guid.NewGuid().ToString())
            .RuleFor(t => t.Type, f => f.PickRandom<TicketType>());

        TicketService sut = new TicketService(context);

        // Act
        TicketResponse? response = sut.Update(ticketId, updatedTicket);

        // Assert
        response.Should().NotBeNull();
        response.CustomerEmail.Should().Be(updatedTicket.CustomerEmail);
        response.CustomerNumber.Should().Be(updatedTicket.CustomerNumber);
        response.CustomerPhone.Should().Be(updatedTicket.CustomerPhone);
        response.Description.Should().Be(updatedTicket.Description);
        response.Type.Should().Be(updatedTicket.Type);
    }

    [Fact]
    public void Delete_ExistTicket_ReturnVoid()
    {
        // Arrange
        DataContext context = new DataContext();

        int ticketId = 1;

        Ticket ticket = new Faker<Ticket>()
            .RuleFor(t => t.Id, ticketId)
            .RuleFor(t => t.CreatedAt, DateTime.UtcNow)
            .RuleFor(t => t.CustomerEmail, f => f.Internet.ExampleEmail())
            .RuleFor(t => t.CustomerNumber, f => f.Random.Number().ToString())
            .RuleFor(t => t.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(t => t.Description, Guid.NewGuid().ToString())
            .RuleFor(t => t.Status, f => f.PickRandom<TicketStatus>())
            .RuleFor(t => t.Type, f => f.PickRandom<TicketType>());

        context.Tickets.Add(ticket);

        TicketService sut = new TicketService(context);

        // Act
        sut.Delete(ticketId);

        Ticket? returnedTicket = context.Tickets.FirstOrDefault(i => i.Id == ticketId);

        // Assert
        returnedTicket.Should().BeNull();
    }
}