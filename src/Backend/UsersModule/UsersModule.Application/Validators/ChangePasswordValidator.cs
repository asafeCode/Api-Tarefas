using FluentValidation;
using UsersModule.Application.SharedValidators;
using UsersModule.Application.UseCases.User.ChangePassword;

namespace UsersModule.Application.Validators;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<ChangePasswordCommand>());
    }
}