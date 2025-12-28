namespace Khdamatk.Server.Statics.Regex;

public class UserRegex
{
    public const string PasswordRegex = @"^(?=.*\p{Lu})(?=.*\p{Ll})(?=.*\d)(?=.*[^\p{L}\p{N}\s])(?!.*\s)(?!.*(.)\1\1).+$";
    public const string UserName = @"^[a-zA-Z\s_\-\.]+$";
}