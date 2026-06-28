using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Contracts.Reports;
using Microsoft.EntityFrameworkCore;

namespace Khdamatk.Server.Services
{
    public class ReportService (Database db): IReportsService
    {
        private readonly Database _db = db;



        public async Task<IEnumerable<Report>> GetAllReportsAsync() =>
            await _db.Reports.ToListAsync();

        public async Task<Report> GetReportByIdAsync(int id) =>
            await _db.Reports.FindAsync(id);

        public async Task AddReportAsync(CreateReportRequest report)
        {
            var r = report.Adapt<Report>();
            _db.Reports.Add(r);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteReportAsync(int id)
        {
            var report = await _db.Reports.FindAsync(id);
            if (report != null) { _db.Reports.Remove(report); await _db.SaveChangesAsync(); }
        }
    }
}