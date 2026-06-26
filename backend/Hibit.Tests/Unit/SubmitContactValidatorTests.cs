using FluentAssertions;
using FluentValidation.TestHelper;
using Hibit.Application.Contact;

namespace Hibit.Tests.Unit;

public class SubmitContactValidatorTests
{
    private readonly SubmitContactValidator _validator = new();

    [Fact]
    public void Should_Pass_For_Valid_Command()
    {
        var command = CreateValidCommand();
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Consent_Not_Given()
    {
        var command = CreateValidCommand() with { ConsentGiven = false };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ConsentGiven);
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var command = CreateValidCommand() with { Email = "invalid" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        var command = CreateValidCommand() with { Name = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    private static SubmitContactCommand CreateValidCommand() => new(
        Name: "João Silva",
        Email: "joao@example.com",
        Phone: "11999999999",
        Subject: "Orçamento",
        Message: "Gostaria de mais informações.",
        ConsentGiven: true);
}
