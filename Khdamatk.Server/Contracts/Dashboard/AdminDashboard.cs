namespace Khdamatk.Server.Contracts.Dashboard;

///  High-level system statistics for the main Admin Dashboard. 
public record AdminStatsResponse(
    int TotalUsers,
    int TotalFreelancers,
    int ActiveFreelancers,
    int NewUsersToday,
    int ReportedUsers,
    int PendingReports
);

///  statistics specific to the Report Management screen. 
public record ReportStatsResponse(int TotalReports, int OpenReports, int ResolvedReports);