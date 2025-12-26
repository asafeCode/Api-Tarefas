using FluentValidation;
using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using UsersModule.Application.SharedValidators;
using UsersModule.Domain.Extensions;

namespace UsersModule.Application.UseCases.Auth.Register;

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