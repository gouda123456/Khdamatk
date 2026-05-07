using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khdamatk.Server.Services;

public class AdminDashboardService(Database db) : IAdminDashboardSerivce
{
    private readonly Database _db = db;

    public async Task<resultBase> GetAdminStats(CancellationToken ct)
    {
        // سحب الأرقام الحقيقية بناءً على الأرقام اللي في الصورة
        var totalUsers = await db.Users.CountAsync(ct);
        var totalFreelancers = await db.Users.CountAsync(u => u.Role == "Freelancer", ct);
        var totalClients = await db.Users.CountAsync(u => u.Role == "Client", ct);
        var newUsers = await db.Users.CountAsync(u => u.CreatedAt >= DateTime.Today.AddDays(-7), ct);
        var bannedUsers = await db.Users.CountAsync(u => u.Status == "Blocked", ct);
        var pendingReports = await db.Reports.CountAsync(r => r.Status == "Pending", ct);

        // ملحوظة: لو مفيش ImageUrl في كلاس الـ User عندك، بنبعت null
        var recentUsers = await db.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(4)
            .Select(u => new RecentUserDto(u.FullName, u.Role, u.CreatedAt, u.ProfilePicture.FullPath))
            .ToListAsync(ct);

        // هنا استخدمنا ClientName كبديل لـ ReporterName بناءً على الكلاس بتاعك
        var recentReports = await db.Reports
            .OrderByDescending(r => r.CreatedAt)
            .Take(4)
            .Select(r => new RecentReportDto(r.Id, r.Type, r.ClientName, r.Status, r.CreatedAt))
            .ToListAsync(ct);




        // تجميع كل الداتا في الـ Response
        var stats = new AdminStatsResponse(
            totalUsers,
            totalFreelancers,
            totalClients,
            newUsers,
            bannedUsers,
            pendingReports,
            recentUsers,   
            recentReports 
        );

        return Success(StatusCodes.Status200OK, stats);
    }
}