using Khdamatk.Server.Contracts.orders;
using Khdamatk.Server.Contracts.WebHook;

namespace Khdamatk.Server.Services.Interfaces;

public interface IJobOrderService
{

    /*TODOs:
     * GetMyJobs()         // للـ Customer
     * GetMyOffers()       // للـ Freelancer  
     * GetMyOrders()       // للاتنين
     * RateAndReview()     // بعد إكمال الأوردر
     * CloseDispute()      // للـ Admin
     */


    Task<resultBase> AddJobASync(AddJobRequest request, CancellationToken cancellationToken);
    Task<resultBase> AddOfferAsync(int JobId, AddJopOfferRequest request, CancellationToken cancellationToken);
    
    //TODO: Add Advanced Search to offer 
    Task<resultBase> ShowOffersJob(int JobId, CancellationToken cancellationToken);
    
    Task<resultBase> RejectOfferJob(int jobId, int offerId, CancellationToken cancellationToken);
    Task<resultBase> ViewOfferDetails(int jobId, int offerId, CancellationToken cancellationToken);
    Task<resultBase> ChangeSelectionOfferJob(int jobId, int oldOfferId, int newOfferId,string userId, CancellationToken cancellationToken);
    Task<resultBase> StartJobOrder(int jobId, int offerId, CancellationToken cancellationToken);
    
    Task<resultBase> CancelJobOrder(int orderId, string userId, CancellationToken cancellationToken);
    Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken);
    Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken);
    Task<resultBase> OrderSummary(int orderId,string userId);
    Task<resultBase> OrderDetails(int orderId, string userId);
    Task<resultBase> SubmitWorkAndMessage(int orderId, SubmitWorkAndMessageRequest request);
    Task<resultBase> CompleteJobOrder(int orderId, CancellationToken cancellationToken);
    Task<resultBase> RevisionJobOrder(int orderId, CancellationToken cancellationToken);

    Task<resultBase> OpenDispute(int orderId,string RaiserId, string ReasonDetails, CancellationToken cancellationToken);

    
}

