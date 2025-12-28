namespace Khdamatk.Server.Statics.Errors;

public static class UserErrors
{
    public const string Title = "Invalid User Authentication Operation";
    public const string Message = "There was an error during the user authentication process.";

    public static Error UserNotFound = new(nameof(UserNotFound), "The user with the provided email does not exist.");
    public static Error InvalidPassword = new(nameof(InvalidPassword), "The provided password is incorrect.");
    public static Error UserAlreadyExists = new(nameof(UserAlreadyExists), "A user with the provided email already exists.");
    public static Error RoleNotFound = new(nameof(RoleNotFound), "The specified role does not exist.");
    public static Error PermissionDenied = new(nameof(PermissionDenied), "You do not have permission to perform this action.");
    public static Error RefreshTokenDoesNotExists = new("Wrong RefreshToken", "you send a Wrong RefreshToken");
    public static Error InvalidRefreshToken = new Error("the refresh token is not valid please sign in", "the refresh token is either Uesed or Expired or revoked we recommen you to re login  ");
    public static Error EmailAlreadyExist = new("there are registered user to this Email ", "if its not you call the support team");
    public static Error InvalidEmailToken = new Error("invalid email conformation Token", "the code you sent is wrong");
    public static Error EmailIsAlreadyConfirmed = new("email is confirmed", "there is no need to confirm this email its already confirmed");
    public static Error EmailServiceNotWorking = new("there are Error while sending email", "we are sorry there is an issue with our email service please try again later or contact support");
}
