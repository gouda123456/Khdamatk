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

    Task<bool> SendProposalRejectedAsync(string toEmail, string userName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.ProposalRejected, new Dictionary<string, string>
        {
            { "UserName", userName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Proposal Rejected", template);
    }

    Task<bool> SendReportApprovalAsync(string toEmail, string userName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.ReportApproval, new Dictionary<string, string>
        {
            { "UserName", userName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Report Approved", template);
    }

    Task<bool> SendReportRejectionAsync(string toEmail, string userName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.ReportRejection, new Dictionary<string, string>
        {
            { "UserName", userName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Report Rejected", template);
    }

    Task<bool> SendReportResolvedAsync(string toEmail, string userName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.ReportResolved, new Dictionary<string, string>
        {
            { "UserName", userName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Report Resolved", template);
    }

    Task<bool> SendReportSubmittedAsync(string toEmail, string userName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.ReportSubmitted, new Dictionary<string, string>
        {
            { "UserName", userName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Report Submitted", template);
    }

    Task<bool> SendRevisionRequestAsync(string toEmail, string userName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.RevisionRequest, new Dictionary<string, string>
        {
            { "UserName", userName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Revision Requested", template);
    }

    Task<bool> SendServiceAcceptanceAsync(string toEmail, string userName, string clientName, string serviceName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.ServiceAcceptance, new Dictionary<string, string>
        {
            { "UserName", userName },
            { "ClientName", clientName },
            { "ServiceName", serviceName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Service Acceptance Letter", template);
    }

    Task<bool> SendUserNotificationAsync(string toEmail, string userName, string notificationMessage)
    {
        var template = GetEmailTemplate(EmailTemplatesName.UserNotification, new Dictionary<string, string>
        {
            { "UserName", userName },
            { "NotificationMessage", notificationMessage }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Notification", template);
    }

    Task<bool> SendVerifyEmailAsync(string toEmail, string userName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.VerifyEmail, new Dictionary<string, string>
        {
            { "UserName", userName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Verify Your Email", template);
    }

    Task<bool> SendWorkDeliveryAsync(string toEmail, string userName)
    {
        var template = GetEmailTemplate(EmailTemplatesName.WorkDelivery, new Dictionary<string, string>
        {
            { "UserName", userName }
        });
        if (template == null) return Task.FromResult(false);
        return SendEmailAsync(toEmail, "Work Delivered", template);
    }

}