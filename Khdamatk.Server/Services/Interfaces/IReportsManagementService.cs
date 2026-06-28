using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Contracts.Reports;

namespace Khdamatk.Server.Services
{
    public interface IReportsService
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(int id);
        Task AddReportAsync(CreateReportRequest report);
        Task DeleteReportAsync(int id);
    }
}