using FluentAssertions;
using FluentValidation.TestHelper;
using Hibit.Application.Auth;

namespace Hibit.Tests.Unit;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator = new();

    [Fact]
    public void Should_Pass_For_Valid_Command()
    {
        var command = new LoginCommand("hibit-app", "password123");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Username_Is_Empty()
    {
        var command = new LoginCommand("", "password123");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Empty()
    {
        var command = new LoginCommand("hibit-app", "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
