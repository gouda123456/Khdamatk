using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Services.Interfaces;

namespace Khdamatk.Server.Services;

public class UserDashboardSerivce(Database db) : IUserDashboardService
{
    private readonly Database _db = db;

    public async Task<resultBase> GetUsersList(string? search, string? role, string? status, CancellationToken ct)
    {
        var users = new List<UserListItem>
        {
            new ("#001", "Sarah Johnson", "sarah.j@email.com", "Freelancer", 12, "Active", DateTime.Now),
            new ("#002", "Michael Chen", "m.chen@email.com", "User", 3, "Verified", DateTime.Now),
            new ("#003", "Emma Davis", "emma.d@email.com", "Freelancer", 8, "Blocked", DateTime.Now),
            new ("#004", "James Wilson", "j.wilson@email.com", "User", 1, "Active", DateTime.Now),
            new ("#005", "Lisa Rodriguez", "lisa.r@email.com", "Freelancer", 15, "Verified", DateTime.Now)
        };

        var filtered = users.Where(u =>
            (string.IsNullOrEmpty(search) || u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) || u.Email.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(role) || u.Role == role) &&
            (string.IsNullOrEmpty(status) || u.Status == status)
        ).ToList();

        return Success(StatusCodes.Status200OK, filtered);
    }

    public async Task<resultBase> SetUserRole(UpdateRoleRequest request) =>
        Success(StatusCodes.Status200OK, "User role updated successfully");

    public async Task<resultBase> UpdateUserStatus(UpdateUserStatusRequest request) =>
        Success(StatusCodes.Status200OK, "User status updated");
}