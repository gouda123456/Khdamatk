namespace Khdamatk.Server.Helper;

public interface IEmailHelper
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    string? GetEmailTemplate(EmailTemplatesName TemplateName, Dictionary<string, string> keyValuePairs);

    Task<bool> SendConfirmationEmailAsync(string toEmail, string userName, string confirmationLink)
    {
        var template = GetEmailTemplate(EmailTemplatesName.ConfirmEmail, new Dictionary<string, string>
        {
            { "UserName", userName },
            { "ConfirmationLink", confirmationLink }
        });
        if (template == null)
        {
            return Task.FromResult(false);
        }
        return SendEmailAsync(toEmail, "Confirm your email", template);
    }

    Task<bool> SendresetPasswordEmailAsync(string toEmail, int code)
    {
        var template = GetEmailTemplate(EmailTemplatesName.ResetPassword, new Dictionary<string, string>
        {
            { "code", code.ToString() },
            
        });
        if (template == null)
        {
            return Task.FromResult(false);
        }
        return SendEmailAsync(toEmail, "Reset your password", template);
    }

}