namespace Khdamatk.Server.Contracts.Authentications;

public record SetPasswordRequest(
    string CurrentPassword,
    string NewPassword
);

public class SetPasswordRequestValidator : AbstractValidator<SetPasswordRequest>
{
    public SetPasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .SetValidator(new PasswordValidator());

        RuleFor(x => x.NewPassword)
            .SetValidator(new PasswordValidator());
        
        RuleFor(x => x.NewPassword)
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("New password must be different from the current password.");

    }
}

