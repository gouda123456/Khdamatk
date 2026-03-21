namespace Khdamatk.Server.Contracts.Dashboard;

public record AdminStatsResponse(
    int TotalUsers,
    int TotalFreelancers,
    int TotalClients,
    int NewUsers,
    int BannedUsers,
    int PendingReports,
    List<RecentUserDto> RecentUsers,
    List<RecentReportDto> RecentReports
);

public record RecentUserDto(string FullName, string Role, DateTime CreatedAt, string? ImageUrl);
public record RecentReportDto(string Id, string Type, string ReporterName, string Status, DateTime CreatedAt);
///  statistics specific to the Report Management screen. 
public record ReportStatsResponse(int TotalReports, int OpenReports, int ResolvedReports);