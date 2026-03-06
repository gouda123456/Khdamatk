namespace Khdamatk.Server.Services.Interfaces;

public interface IJobOrderService
{
    Task<resultBase> AddJobASync(AddJobRequest request, CancellationToken cancellationToken);
    Task<resultBase> AddOfferAsync(int JobId, AddJopOfferRequest request, CancellationToken cancellationToken);
    
    //TODO: Add Advanced Search to offer 
    Task<resultBase> ShowOffersJob(int JobId, CancellationToken cancellationToken);
    Task<resultBase> AcceptOfferJob(int jobId, int offerId, CancellationToken cancellationToken);
    Task<resultBase> RejectOfferJob(int jobId, int offerId, CancellationToken cancellationToken);
    Task<resultBase> ViewOfferDetails(int jobId, int offerId, CancellationToken cancellationToken);
    Task<resultBase> ChangeSelectionOfferJob(int jobId, int oldOfferId, int newOfferId, CancellationToken cancellationToken);
    Task<resultBase> StartJobOrder(int jobId, int offerId, CancellationToken cancellationToken);
    Task<resultBase> PayJobOrder(int orderId, CancellationToken cancellationToken);
    Task<resultBase> CancelJobOrder(int orderId, OrderStatus orderStatus, CancellationToken cancellationToken);
    Task<resultBase> PaymentSuccessJobOrder(int orderId, CancellationToken cancellationToken);
    Task<resultBase> PaymentFailureJobOrder(int orderId, CancellationToken cancellationToken);
    Task<resultBase> CompleteJobOrder(int orderId, CancellationToken cancellationToken);

    
}
