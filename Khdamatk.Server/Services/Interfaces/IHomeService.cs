namespace Khdamatk.Server.Services.Interfaces;

public interface IHomeService : IService
{
    Task<resultBase> MainPage(CancellationToken cancellationToken);
    Task<resultBase> JobsPage(string? service, CancellationToken cancellationToken);

}
