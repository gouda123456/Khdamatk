using MimeKit;

namespace Khdamatk.Server.Helper;

public class EmailHelper(
    IWebHostEnvironment env,
    IOptions<EmailSetting> setting
    ) : IEmailHelper
{
    private readonly IWebHostEnvironment env = env;

    private readonly EmailSetting setting = setting.Value;
    private static Dictionary<string, string> EmailsBody = [];


    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {

            var message = new MimeMessage()
            {
                Subject = subject,
                Body = new TextPart("html")
                {
                    Text = body
                }
            };
            message.From.Add(new MailboxAddress(setting.Email, setting.Email));
            message.To.Add(MailboxAddress.Parse(toEmail));

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(setting.Host, setting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(setting.Email, setting.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            return true;

        }
        catch
        {
            return false;

        }
    }

    //TODO: Caching the templates in memory to avoid reading from disk every time
    //TODO: Add logging for missing templates
    //TODO: Change TemplateName Type to Enum
    public string? GetEmailTemplate(EmailTemplatesName TemplateName, Dictionary<string, string> keyValuePairs)
    {
        // !false means we found it , true means we did not find it
        if (!EmailsBody.TryGetValue(TemplateName.ToString(), out string? content) || string.IsNullOrEmpty(content))
        {
            var path = Path.Combine(env.ContentRootPath, "Templates", $"{TemplateName.ToString()}.html");
            if (!File.Exists(path))
            {
                return null;
            }
            using (var reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
                EmailsBody[TemplateName.ToString()] = content;
            }
        }

        foreach (var pair in keyValuePairs)
        {
            content = content.Replace($"&&{pair.Key}&&", pair.Value);
        }

        return content;

    }


}


