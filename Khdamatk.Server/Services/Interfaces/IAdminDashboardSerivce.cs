using Khdamatk.Server.Contracts.Dashboard;

namespace Khdamatk.Server.Services.Interfaces;

public interface IAdminDashboardSerivce : IService
{
    ///  Retrieves core platform numbers (Total Users, Freelancers, etc.). 
    Task<resultBase> GetAdminStats(CancellationToken ct);
}