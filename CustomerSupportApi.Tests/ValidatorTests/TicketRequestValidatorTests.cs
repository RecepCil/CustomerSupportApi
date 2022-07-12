using Bogus;
using CustomerSupportApi.Models.Requests;
using CustomerSupportApi.Models.Requests.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace CustomerSupportApi.Tests.ValidatorTests;

public class TicketRequestValidatorTests
{
    [Fact]
    public void Validate_RequestIsEmpty_ReturnError()
    {
        // Arrange
        TicketRequestValidator sut = new TicketRequestValidator();

        // Act
        ValidationResult? validationResult = sut.Validate(new TicketRequest());

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
    }
    
    [Fact]
    public void Validate_CustomerEmailIsEmpty_ReturnError()
    {
        // Arrange
        TicketRequestValidator sut = new TicketRequestValidator();

        TicketRequest request = new TicketRequest() {};
        
        // Act
        var validationResult = sut.TestValidate(request);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
        validationResult.ShouldHaveValidationErrorFor(x => x.CustomerEmail);
    }
    
    [Theory]
    [InlineData("@gmail.com")]
    [InlineData("gmail.com")]
    public void Validate_CustomerEmailIsNotValidEmailAddress_ReturnError(string customerEmail)
    {
        // Arrange
        TicketRequestValidator sut = new TicketRequestValidator();

        TicketRequest request = new TicketRequest()
        {
            CustomerEmail = customerEmail
        };
        
        // Act
        var validationResult = sut.TestValidate(request);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
        validationResult.ShouldHaveValidationErrorFor(x => x.CustomerEmail);
    }
    
    [Fact]
    public void Validate_CustomerPhoneIsEmpty_ReturnError()
    {
        // Arrange
        TicketRequestValidator sut = new TicketRequestValidator();

        TicketRequest request = new Faker<TicketRequest>()
            .RuleFor(r => r.CustomerEmail, f => f.Internet.Email());
        
        // Act
        var validationResult = sut.TestValidate(request);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
        validationResult.ShouldHaveValidationErrorFor(x => x.CustomerPhone);
    }
    
    [Fact]
    public void Validate_CustomerNumberIsEmpty_ReturnIsValidTrue()
    {
        // Arrange
        TicketRequestValidator sut = new TicketRequestValidator();

        TicketRequest request = new Faker<TicketRequest>()
            .RuleFor(r => r.CustomerEmail, f => f.Internet.Email())
            .RuleFor(r => r.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(r => r.Description, f => f.Random.Word());
        
        // Act
        var validationResult = sut.TestValidate(request);

        // Assert
        validationResult.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Validate_CustomerNumberIsAlphaNumeric_ReturnIsValidTrue()
    {
        // Arrange
        TicketRequestValidator sut = new TicketRequestValidator();

        TicketRequest request = new Faker<TicketRequest>()
            .RuleFor(r => r.CustomerEmail, f => f.Internet.Email())
            .RuleFor(r => r.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(r => r.CustomerNumber, f => f.Random.Word())
            .RuleFor(r => r.Description, f => f.Random.Word());
        
        // Act
        var validationResult = sut.TestValidate(request);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
        validationResult.ShouldHaveValidationErrorFor(x => x.CustomerNumber);
    }
    
    [Fact]
    public void Validate_DescriptionIsLongerThanExpected_ReturnIsValidTrue()
    {
        // Arrange
        TicketRequestValidator sut = new TicketRequestValidator();

        TicketRequest request = new Faker<TicketRequest>()
            .RuleFor(r => r.CustomerEmail, f => f.Internet.Email())
            .RuleFor(r => r.CustomerPhone, f => f.Phone.PhoneNumber())
            .RuleFor(r => r.Description, f => f.Random.String2(501));
        
        // Act
        var validationResult = sut.TestValidate(request);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
        validationResult.ShouldHaveValidationErrorFor(x => x.Description);
    }
}