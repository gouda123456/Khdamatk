namespace Khdamatk.Server.Statics.MessagesRespond;

public class UserMessagesRespond
{
    // General success Messages
    public const string LoginSuccessful = "Login successful. Welcome back!";
    public const string RegistrationSuccessful = "Registration completed successfully. Please check your email to confirm your account.";
    public const string EmailConfirmed = "Your email has been confirmed successfully. You can now log in to your account.";
    public const string ConfirmationEmailResent = "A new confirmation email has been sent to your email address. Please check your inbox.";
    public const string TokenRefreshed = "Your token has been refreshed successfully.";


    // General failure Messages
    public const string InvalidCredentials = "The email or password you entered is incorrect. Please try again.";
    public const string AccountAlreadyExists = "An account with this email already exists. Please use a different email or sign in.";
}
