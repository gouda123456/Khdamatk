namespace Khdamatk.Server.Contracts.Admin.Verification;

public class AdminVerificationResponse
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty; // Freelancer أو Client
    public string NationalNumber { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string IdFrontUrl { get; set; } = string.Empty;
    public string IdBackUrl { get; set; } = string.Empty;
    public string SelfieWithIdUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
}
