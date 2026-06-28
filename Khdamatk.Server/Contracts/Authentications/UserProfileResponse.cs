namespace Khdamatk.Server.Contracts.Authentications;

public record UserProfileResponse(
    string Id,
    string FullName,
    string Email,
    string Address,
    string DateOfJoining,
    decimal Rate,
    int TotalJobs,
    int freelancerHiredCount,
    decimal Amonunt,
    string profilePic,
    List<UserRpfilePostedJobs> PostedJobs,
    List<UserRpfileHiringHistory> HiringHistories,
    VerifyCardUserProfile Verify,
    UserRpfileFinancial Financial
    );

public record UserRpfilePostedJobs(
    string Categoryicon,
    string CategoryName,
    string JobTitle,
    decimal Amount,
    JobPostStatus Status
    );

public record UserRpfileHiringHistory(
    string FreelancePic,
    string name,
    decimal Amount,
    OrderStatus Status,
    DateTime CreateAt
    );

public record VerifyCardUserProfile(
    bool Email,
    bool phone,
    bool Identity,
    bool Business
    );

public record UserRpfileFinancial(
    decimal WalletBalance,
    decimal TotalSpendBalance,
    List<UserRpfileTransaction> Transactions
    );

public record UserRpfileTransaction(
    string title,
    decimal Amount,
    DateTime CreatedDate
    );

