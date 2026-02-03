using Khdamatk.Server.Contracts.Home;

namespace Khdamatk.Server.Services.Interfaces
{
    public interface IHomeService: IService
    {
        Task<HomeMainResponse> HomeMain();
        
    }
}
