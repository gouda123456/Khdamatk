using Khdamatk.Server.Contracts.Service;

namespace Khdamatk.Server.Services.Implementations;

public class ServiceOrderService : IServiceOrderService
{
    public async Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * 
         */
        return Failure(StatusCodes.Status501NotImplemented,FailureMessages.NotImplemented.Title,FailureMessages.NotImplemented.Message);
    }

    public async Task<resultBase> GetServiceAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }

    public async Task<resultBase> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> UpdateServiceAsync(int serviceId, AddServiceRequest request, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> AddOrderAsync(int ServiceId, string CustomerId, OrderServiceRequest request, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> FreeLancerAcceptOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> FreelancerRejectOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> PayOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> GetOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> CancelOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> AriseDisputeAsync(int orderId, string reason, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
}
