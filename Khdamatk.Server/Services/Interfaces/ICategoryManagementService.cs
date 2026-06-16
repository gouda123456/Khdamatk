namespace Khdamatk.Server.Services.Interfaces
{
    public interface ICategoryManagementService:IService
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        Task<Category?> UpdateAsync(int id, Category category);

        Task<bool> DeleteAsync(int id);

    }
}
