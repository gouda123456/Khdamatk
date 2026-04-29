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


    Task<resultBase> AddJobASync(AddJobRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> AddOfferAsync(int JobId, AddJopOfferRequest request, CancellationToken cancellationToken = default);
    
    //TODO: Add Advanced Search to offer 
    Task<resultBase> ShowOffersJob(int JobId, CancellationToken cancellationToken = default);
    
    Task<resultBase> RejectOfferJob(int jobId, int offerId, CancellationToken cancellationToken = default);
    Task<resultBase> ViewOfferDetails(int jobId, int offerId, CancellationToken cancellationToken = default);
    Task<resultBase> ChangeSelectionOfferJob(int jobId, int oldOfferId, int newOfferId,string userId, CancellationToken cancellationToken = default);
    Task<resultBase> StartJobOrder(int jobId, int offerId, CancellationToken cancellationToken = default);
    
    Task<resultBase> CancelJobOrder(int orderId, string userId, CancellationToken cancellationToken = default);
    Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken = default);
    Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken = default);
    Task<resultBase> OrderSummary(int orderId,string userId);
    Task<resultBase> OrderDetails(int orderId, string userId);
    Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversationMessages(int OrderId, string UserId, CancellationToken cancellationToken = default);
    
    Task<resultBase> CompleteJobOrder(int orderId, ReviewRequest request, CancellationToken cancellationToken = default);
    

    Task<resultBase> OpenDispute(int orderId,string RaiserId, string ReasonDetails, CancellationToken cancellationToken = default);

    // 1. إضافة طلب جديد (العميل بيبعت طلب لـ Freelancer معين)
    // في الـ IJobOrderService
    Task<resultBase> AddOrderAsync(CreateJobOrderRequest request, string customerId, CancellationToken cancellationToken = default);

    // 2. قبول الطلب من طرف الـ Freelancer
    Task<resultBase> AcceptOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default);

    // 3. رفض الطلب من طرف الـ Freelancer
    Task<resultBase> RejectOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default);
    // 1. الميثود الخاصة بجلب أوردر واحد محدد
    Task<resultBase> GetOrderById(int id, string userId);

    // 2. الميثود الخاصة بجلب كل أوردرات المستخدم
    Task<resultBase> GetUserOrders(string userId);

}

