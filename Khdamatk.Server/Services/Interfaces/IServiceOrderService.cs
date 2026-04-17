using Khdamatk.Server.Contracts.Service;

namespace Khdamatk.Server.Services.Interfaces;

public interface IServiceOrderService : IService
{
    /*TODOs:
     * CRUD:
    * {
        * AddService,
        * GetService,
        * GetServices,
        * UpdateService,
        * DeleteService
    * }
     * 
     * Order:
    * {
        * addOrder,
        * FreeLancerAcceptOrder,
        * FreelancerRejectOrder,
        * PayOrder,
        * GetOrder,
        * GetOrders,
        * SubmitWork(files or message and assign to userId),
        * CompleteOrder,
        * CancelOrder,
        * AriseDispute,
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
    Task<resultBase> GetOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> GetOrdersAsync(CancellationToken cancellationToken = default);
    Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> CancelOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> AriseDisputeAsync(int orderId, string reason, CancellationToken cancellationToken = default);


}
