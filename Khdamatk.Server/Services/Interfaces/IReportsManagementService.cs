using Khdamatk.Server.Contracts.Dashboard;

namespace Khdamatk.Server.Services
{
    public interface IReportsService
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(int id);
        Task AddReportAsync(Report report);
        Task DeleteReportAsync(int id);
    }
}