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

    Task<bool> SendJobCompletedAsync(string toEmail, string userName, string jobName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.JobCompleted, new Dictionary<string, string>
        {
            { "UserName", userName },
            { "JobName", jobName }
        });
        if (template == null)
        {
            return Task.FromResult(false);
        }
        return SendEmailAsync(toEmail, "Job Completed", template);
    }

    Task<bool> SendJobInProgressAsync(string toEmail, string userName, string jobName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.JobInProgress, new Dictionary<string, string>
        {
            { "UserName", userName },
            { "JobName", jobName }
        });
        if (template == null)
        {
            return Task.FromResult(false);
        }
        return SendEmailAsync(toEmail, "Job In Progress", template);
    }

    Task<bool> SendJobPostConfirmationAsync(string toEmail, string userName, string jobName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.JobPostConfirmation, new Dictionary<string, string>
        {
            { "UserName", userName },
            { "JobName", jobName }
        });
        if (template == null)
        {
            return Task.FromResult(false);
        }
        return SendEmailAsync(toEmail, "Job Post Confirmation", template);
    }

    Task<bool> SendMilestoneCompletedAsync(string toEmail, string userName, string jobName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.MilestoneCompleted, new Dictionary<string, string>
        {
            { "UserName", userName },
            { "JobName", jobName }
        });
        if (template == null)
        {
            return Task.FromResult(false);
        }
        return SendEmailAsync(toEmail, "Milestone Completed", template);
    }

    Task<bool> SendNewProposalAsync(string toEmail, string userName, string jobTitle)
    {
        var template = GetEmailTemplate(EmailTemplatesName.NewProposal, new Dictionary<string, string>
        {
            { "UserName", userName },
            { "JobTitle", jobTitle }
        });
        if (template == null)
        {
            return Task.FromResult(false);
        }
        return SendEmailAsync(toEmail, "New Proposal Received", template);
    }



}