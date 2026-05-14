using Khdamatk.Server.Contracts.Service;
using Khdamatk.Server.Contracts.WebHook;

namespace Khdamatk.Server.Services.Interfaces;

public interface IServiceOrderService : IService
{
    /*TODOs:
     * CRUD:
    * {
        * AddService,       S Done
        * GetService,       S Done
        * GetServices,      S Done
        * UpdateService,    S Done
        * DeleteService     S Done
    * }
     * 
     * Order:
    * {
        * addOrder,                 A Done
        * FreeLancerAcceptOrder,    A Done
        * FreelancerRejectOrder,    A Done
        * PayOrder,                     G Done
        * PaymentSuccess,               G Done
        * PaymentFailure,               G
        * GetOrder,                 A Done
        * GetOrders,                A Done
        * SubmitWork(files or message and assign to userId),        G Done
        * CompleteOrder,        G Done
        * CancelOrder,          G
        * AriseDispute,         G Done
    * }
     */

    Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> GetServiceAsync(int serviceId, CancellationToken cancellationToken = default);
    Task<resultBase> GetServicesAsync(CancellationToken cancellationToken = default);
    Task<resultBase> UpdateServiceAsync(int serviceId, AddServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default);
    Task<resultBase> AddOrderAsync(int ServiceId,string CustomerId,OrderServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> FreeLancerAcceptOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> FreelancerRejectOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> PayOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken = default);
    Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken = default);
    Task<resultBase> GetOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> GetOrdersAsync(CancellationToken cancellationToken = default);
    Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversationMessages(int orderId, string UserId, CancellationToken cancellationToken = default);
    Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> CancelOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> OpenDispute(int orderId, string RaiserId, string ReasonDetails, CancellationToken cancellationToken = default);
    // 1. إضافة طلب جديد (العميل بيبعت طلب لـ Freelancer معين)
    // في الـ IJobOrderService
    Task<resultBase> AddOrderAsync1(CreateJobOrderRequest request, string customerId, CancellationToken cancellationToken = default);

    // 2. قبول الطلب من طرف الـ Freelancer
    Task<resultBase> AcceptOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default);

    // 3. رفض الطلب من طرف الـ Freelancer
    Task<resultBase> RejectOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default);
    // 1. الميثود الخاصة بجلب أوردر واحد محدد
    Task<resultBase> GetOrderById(int id, string userId);

    // 2. الميثود الخاصة بجلب كل أوردرات المستخدم
    Task<resultBase> GetUserOrders(string userId);


    ////////s//////

    // إضافة خدمة جديدة
    Task<resultBase> AddService(AddServiceRequest1 request, CancellationToken ct);

    // تعديل خدمة موجودة
    Task<resultBase> UpdateService(int id, UpdateServiceRequest request, CancellationToken ct);

    // حذف خدمة (غالباً بيكون Soft Delete)
    Task<resultBase> DeleteService(int id, CancellationToken ct);

    // الحصول على تفاصيل خدمة واحدة بالـ ID
    Task<resultBase> GetServiceById(int id, CancellationToken ct);

    // الحصول على كل الخدمات مع دعم البحث والفلترة
    // لو الـ request كان فاضي بيرجع كل الداتا
    Task<resultBase> GetServices(GetServicesRequest request, CancellationToken ct);

}
