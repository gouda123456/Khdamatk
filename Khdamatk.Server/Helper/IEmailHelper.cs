namespace Khdamatk.Server.Helper;

public interface IEmailHelper
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    string? GetEmailTemplate(EmailTemplatesName TemplateName, Dictionary<string, string> keyValuePairs);

}