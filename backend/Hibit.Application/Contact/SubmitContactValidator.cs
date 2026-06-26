using FluentValidation;

namespace Hibit.Application.Contact;

public class SubmitContactValidator : AbstractValidator<SubmitContactCommand>
{
    public SubmitContactValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Informe um e-mail válido.")
            .MaximumLength(320).WithMessage("O e-mail deve ter no máximo 320 caracteres.");

        RuleFor(x => x.Phone)
            .MaximumLength(30).WithMessage("O telefone deve ter no máximo 30 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("O assunto é obrigatório.")
            .MaximumLength(200).WithMessage("O assunto deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("A mensagem é obrigatória.")
            .MaximumLength(5000).WithMessage("A mensagem deve ter no máximo 5000 caracteres.");

        RuleFor(x => x.ConsentGiven)
            .Equal(true).WithMessage("É necessário concordar com a Política de Privacidade.");
    }
}
