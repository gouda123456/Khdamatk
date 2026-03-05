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

    public Task<resultBase> GetJobAsync(int jobId)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> GetUsersJobAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
