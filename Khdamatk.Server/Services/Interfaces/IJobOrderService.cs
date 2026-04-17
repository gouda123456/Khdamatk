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
    Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversationMessages(int JobId, string UserId, CancellationToken cancellationToken = default);
    Task<resultBase> CompleteJobOrder(int orderId, ReviewRequest request, CancellationToken cancellationToken);
    

    Task<resultBase> OpenDispute(int orderId,string RaiserId, string ReasonDetails, CancellationToken cancellationToken);

    
}

