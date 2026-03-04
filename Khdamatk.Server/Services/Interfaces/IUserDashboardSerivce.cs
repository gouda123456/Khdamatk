using Khdamatk.Server.Contracts.Dashboard;

namespace Khdamatk.Server.Services.Interfaces;

public interface IUserDashboardService : IService
{
    /// Gets the list of users with search and filter capabilities. 
    Task<resultBase> GetUsersList(string? search, string? role, string? status, CancellationToken ct);

    ///  Updates user status (Verify, Block, Active). 
    Task<resultBase> UpdateUserStatus(UpdateUserStatusRequest request);

    ///  Sets or changes a user's role in the system. 
    Task<resultBase> SetUserRole(UpdateRoleRequest request);
}