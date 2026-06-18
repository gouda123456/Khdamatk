using Khdamatk.Server.Contracts.Service;

namespace Khdamatk.Server.Services.Interfaces;

public interface IServiceService : IService
{
    Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken ct = default);
    Task<resultBase> GetServiceAsync(int serviceId, CancellationToken ct = default);
    Task<resultBase> GetServicesAsync(ServiceFilterRequest request, CancellationToken ct = default);
    Task<resultBase> UpdateServiceAsync(int serviceId, UpdateServiceRequest request, CancellationToken ct = default);
    Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken ct = default);
    Task<resultBase> GetProviderServicesAsync(string providerId, CancellationToken ct = default);
}
