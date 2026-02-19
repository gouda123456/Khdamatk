namespace Khdamatk.Server.Services.Interfaces;

public interface IJobService : IService
{
    //TODO: Add Dynamic Filtering and Pagination
    Task<resultBase> GetAllJobsAsync();
    Task<resultBase> GetJobAsync(int jobId);
    Task<resultBase> GetUsersJobAsync(string userId);
    Task<resultBase> GetCategoryJobAsync(int Category);



    //Task<resultBase> AddJobAsync(string userId, jobAddRequest request);
}
