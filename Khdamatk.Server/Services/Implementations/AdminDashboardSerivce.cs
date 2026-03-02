using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Services.Interfaces;

namespace Khdamatk.Server.Services;

public class AdminDashboardService(Database db) : IAdminDashboardSerivce
{
    private readonly Database _db = db;

    ///  Retrieves core platform numbers. 
    public async Task<resultBase> GetAdminStats(CancellationToken ct)
    {
        // الأرقام مطابقة للصورة الأولى
        var stats = new AdminStatsResponse(12847, 7234, 5613, 342, 87, 24);
        return Success(StatusCodes.Status200OK, stats);
    }
}