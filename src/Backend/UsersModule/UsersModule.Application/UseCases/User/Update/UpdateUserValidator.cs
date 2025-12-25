using FluentValidation;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using UsersModule.Domain.Extensions;

namespace UsersModule.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);

        When(user => user.Email.NotEmpty(), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}