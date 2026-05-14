using Khdamatk.Server.Contracts.Home;
using Microsoft.EntityFrameworkCore;

namespace Khdamatk.Server.Services.Implementations;

public class JobService(Database db) : IJobService
{
    private readonly Database db = db;

    public async Task<resultBase> GetAllJobsAsync()
    {
        var Jobs = await db.JobPosts.AsNoTracking().ProjectToType<JobDetailed>().ToListAsync();
        return Success(StatusCodes.Status200OK, "Jobs retrieved successfully", "Jobs retrieved successfully", Jobs);
    }

    public Task<resultBase> GetCategoryJobAsync(int Category)
    {
        throw new NotImplementedException();
    }

    public async Task<resultBase> GetJobAsync(int jobId)
    {
        var Jobs = await db.JobPosts.AsNoTracking().ProjectToType<JobDetailed>().ToListAsync();
        return Success(StatusCodes.Status200OK, "Jobs retrieved successfully", "Jobs retrieved successfully", Jobs.FirstOrDefault(j => j.Id == jobId));
    }

    public Task<resultBase> GetUsersJobAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<JobsPage> GetJobsAsync(JobsFilterRequest request)
    {
        var query = db.JobPosts.AsQueryable();

        // 🔍 Search
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(j => j.Title.Contains(request.Search));
        }

        // 🧩 Filter by category
        if (request.ServiceId.HasValue)
        {
            query = query.Where(j => j.Id == request.ServiceId);
        }

        // 📄 Pagination
        var jobs = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        // 🔄 Mapping
        var jobCards = jobs.Select(j => new JobCard(
            j.Id,
            j.Title,
            j.Description,
            j.Category.Name,
            j.Deadline,
            (double)j.BudgetMin ,
           (double)j.BudgetMax
        )).ToList();

        var services = await db.Services
            .Select(s => new ServiceItem(s.Id, s.Title))
            .ToListAsync();

        return new JobsPage(services, jobCards);
    }



    

}
