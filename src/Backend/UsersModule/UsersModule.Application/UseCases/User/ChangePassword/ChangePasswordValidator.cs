using FluentValidation;
using UsersModule.Application.SharedValidators;

namespace UsersModule.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<ChangePasswordRequest>());
    }
}