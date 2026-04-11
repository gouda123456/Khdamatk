using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khdamatk.Server.Services;

public class UserDashboardSerivce(Database db) : IUserDashboardService
{
   private readonly Database db = db;
    public async Task<resultBase> GetUserManagementList(CancellationToken ct)
    {
        var users = await db.Users
            .Select(u => new UserListItem(
                u.Id,
                u.FullName,
                u.Email,
                u.Role,
                db.JobPosts.Count(j => j.UserId == u.Id), 
                u.Status,
                u.CreatedAt
            ))
            .ToListAsync(ct);

        return Success(StatusCodes.Status200OK, users);
    }

   
    public async Task<resultBase> UpdateUserStatus(UpdateUserStatusRequest request)
    {
        var user = await db.Users.FindAsync(request.UserId);
        if (user == null) return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists);

        user.Status = request.NewStatus; // "Verified" أو "Blocked"
        await db.SaveChangesAsync();

        return Success(StatusCodes.Status200OK, "Status updated successfully");
    }

    // 3. (Set Role)
    public async Task<resultBase> SetUserRole(UpdateRoleRequest request)
    {
        var user = await db.Users.FindAsync(request.UserId);
        if (user == null) return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists);

        user.Role = request.NewRole;
        await db.SaveChangesAsync();

        return Success(StatusCodes.Status200OK, "The rank has been changed successfully");
    }

    public async Task<resultBase> GetUsersList(string? search, string? role, string? status, CancellationToken ct)
    {
        var query = db.Users.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(u => u.FullName.Contains(search) || u.Email.Contains(search));

        var data = await query
            .Select(u => new UserListItem(u.Id, u.FullName, u.Email, u.Role, db.JobPosts.Count(j => j.UserId == u.Id), u.Status, u.CreatedAt))
            .ToListAsync(ct);

        return Success(StatusCodes.Status200OK, data);
    }
}