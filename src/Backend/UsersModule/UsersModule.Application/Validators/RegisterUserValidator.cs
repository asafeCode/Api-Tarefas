using FluentValidation;
using TarefasCrud.Exceptions;
using UsersModule.Application.SharedValidators;
using UsersModule.Application.UseCases.Auth.Register;
using UsersModule.Domain.Extensions;

namespace UsersModule.Application.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RegisterUserCommand>());

        When(user => user.Email.NotEmpty(), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}