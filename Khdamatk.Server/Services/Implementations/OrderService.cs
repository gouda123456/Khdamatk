using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.Orders;
using Khdamatk.Server.Contracts.WebHook;
using Khdamatk.Server.Helper.Payment;
using Stripe;

namespace Khdamatk.Server.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly Database db;
    private readonly IEmailHelper emailHelper;
    private readonly IFawaterakPaymentHelper fawaterakPaymentHelper;

    public OrderService(Database db, IEmailHelper emailHelper, IFawaterakPaymentHelper fawaterakPaymentHelper)
    {
        this.db = db;
        this.emailHelper = emailHelper;
        this.fawaterakPaymentHelper = fawaterakPaymentHelper;
    }

    public async Task<resultBase> StartServiceOrderPaymentAsync(EInvoiceRequestModel order,string? AdditionalDetails,List<Media> Attachments,int serviceId,string userId)
    {

        if(order.CartItems.Count == 0 || order.CartItems.Count > 1)
            return Failure(StatusCodes.Status409Conflict, "Invalid order", "the order you asked for is either not have service or have more than one");

        

        var service = db.Services
            .Include(s => s.ServiceProviderProfile).ThenInclude(sp => sp.User)
            .FirstOrDefault(
            s => s.Id == serviceId);

        if (service == null)
            return Failure(StatusCodes.Status409Conflict, "Invalid service", "the service you asked for is either not found or the data in order of it is wrong");

        if(!order.CartItems.Any(
                i =>
                i.Name == service.Title &&
                i.Price == service.Price &&
                i.Quantity == 1
                ))
            return Failure(StatusCodes.Status409Conflict, "Invalid order", "the order you asked for is either not have service or have more than one");

        if (order.PayLoad == null)
            return Failure(StatusCodes.Status409Conflict, "Invalid PayLoad", "the order you asked for dosent have payload you must add payload.");

        if (order.PayLoad.Provider == null && service.ServiceProviderProfile != null)
            order.PayLoad.Provider = new ProviderModel()
            {
                Id = service.ServiceProviderProfileId,
                Username = service.ServiceProviderProfile.User!.UserName!,
                Email = service.ServiceProviderProfile.User!.Email!
            };
        else if(order.PayLoad.Provider == null || service.ServiceProviderProfile == null || order.PayLoad.Provider.Id != service.ServiceProviderProfileId)
            return Failure(StatusCodes.Status409Conflict, "Invalid order", "the order tou asked for dosent register to service provider (freelancer) this order cant be ordered");

        if (order.Customer == null || order.Customer.CustomerId != userId)
            return Failure(StatusCodes.Status403Forbidden, FailureMessages.Forbidden.Title, FailureMessages.Forbidden.Message);



        EInvoiceResponseModel.EInvoiceResponseDataModel? result = await fawaterakPaymentHelper.CreateEInvoiceAsync(order);

        if (result == null)
            return Failure(StatusCodes.Status503ServiceUnavailable, FailureMessages.ServiceUnavailable.Title, FailureMessages.ServiceUnavailable.Message, new Error("payment is not available", "the payment gateway doesnt available now try later"));

        ServiceOrder? serviceOrder = new ()
        {
            Amount = order.CartTotal,
            CompletionDate = DateTime.UtcNow.AddDays(service.DeliveryTimeInDays),
            AdditionalDetails = AdditionalDetails ?? "doesnt have addition info",
            Conversation = new Conversation(),
            InvoiceId = result.InvoiceId,
            InvoiceKey = result.InvoiceKey,
            ServiceID = service.Id,
            ServiceProviderId = order.PayLoad.Provider!.Id,
            Status = OrderStatus.PendingPayment,
            CustomerId = order.Customer.CustomerId!,
            MediaAttachments = Attachments ?? []
        };


        db.ServiceOrders.Add(serviceOrder);

        await db.SaveChangesAsync();

        return Success(StatusCodes.Status202Accepted, SuccessMessages.General.Title, SuccessMessages.General.Message, result);

    }

    public async Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> StartJobOrderPaymentAsync(int jobOrderId)
    {
        

        var order = await db.jobOrders
            .Include(o => o.Customer)
            .Include(o => o.ProviderProfile)
                .ThenInclude(p => p.User)
            .Include(o => o.JobPost)
            .FirstOrDefaultAsync(o => o.Id == jobOrderId);

        if (order is null || order.Customer is null || order.ProviderProfile is null || order.JobPost is null)
            return null;

        if (order.Status != OrderStatus.PendingPayment)
            return null;

        var request = new EInvoiceRequestModel
        {
            Currency = "EGP",
            DueDate = DateTime.UtcNow.AddDays(7),
            SendEmail = true,
            Customer = new CustomerModel
            {
                FirstName = order.Customer.UserName ?? string.Empty,
                LastName = string.Empty,
                CustomerId = order.Customer.Id,
                Email = order.Customer.Email ?? string.Empty
            },
            CartItems = new List<CartItemModel>
            {
                new CartItemModel
                {
                    Name = order.JobPost.Title,
                    Quantity = 1,
                    Price = order.Amount
                }
            },
            PayLoad = new InvoicePayload
            {
                OrderId = order.Id,
                OrderType = OrderType.Job,
                Provider = new ProviderModel
                {
                    Id = order.ProviderProfile.UserId,
                    Username = order.ProviderProfile.User.UserName ?? string.Empty,
                    Email = order.ProviderProfile.User.Email ?? string.Empty
                }
            },
            Status = OrderStatus.PendingPayment
        };

        EInvoiceResponseModel.EInvoiceResponseDataModel? response = await fawaterakPaymentHelper.CreateEInvoiceAsync(request);
        if (response is null)
            return null;

        order.InvoiceId = response.InvoiceId;
        order.InvoiceKey = response.InvoiceKey;

        await db.SaveChangesAsync();

        return response;
    }

    public async Task HandlePaymentSuccessAsync(WebHookModel webHookModel)
    {
        var (serviceOrder, jobOrder) = await FindOrderByInvoiceAsync(webHookModel.InvoiceId, webHookModel.InvoiceKey);
        if (serviceOrder is null && jobOrder is null)
            return;

        string? customerEmail = null;
        string orderDescription;

        if (serviceOrder is not null)
        {
            serviceOrder.Status = OrderStatus.Active;
            if (serviceOrder.PaymentTransaction is not null)
            {
                serviceOrder.PaymentTransaction.Status = TransactionStatus.Completed;
                serviceOrder.PaymentTransaction.GatewayUsed = PaymentGateway.Fawry;
            }

            customerEmail = serviceOrder.Customer?.Email;
            orderDescription = $"خدمة رقم {serviceOrder.Id}";
        }
        else
        {
            jobOrder!.Status = OrderStatus.Active;
            if (jobOrder.PaymentTransaction!= null && jobOrder.PaymentTransaction.Any(pt => pt.Status != TransactionStatus.Completed))
            {
                jobOrder.PaymentTransaction.Add(new PaymentTransaction()
                {
                    Amount = jobOrder.Amount,
                    GatewayUsed = ,
                    Status = TransactionStatus.Completed,
                    Currency = CurrencyCode.EGP,
                    
                })
                    //Status = TransactionStatus.Completed;
                //jobOrder.PaymentTransaction.GatewayUsed = PaymentGateway.Fawry;
            }

            customerEmail = jobOrder.Customer?.Email;
            orderDescription = $"طلب عمل رقم {jobOrder.Id}";
        }

        await db.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(customerEmail))
        {
            var subject = "تم الدفع بنجاح";
            var body = $"تم إتمام عملية الدفع بنجاح لـ {orderDescription} (فاتورة رقم {webHookModel.InvoiceId}). شكرًا لاستخدامك خدماتك.";
            await emailHelper.SendEmailAsync(customerEmail, subject, body);
        }
    }

    public async Task HandlePaymentFailedAsync(long invoiceId, string invoiceKey, string? errorMessage)
    {
        var (serviceOrder, jobOrder) = await FindOrderByInvoiceAsync(invoiceId, invoiceKey);
        if (serviceOrder is null && jobOrder is null)
            return;

        string? customerEmail = null;
        string orderDescription;

        if (serviceOrder is not null)
        {
            serviceOrder.Status = OrderStatus.CancelledByClient;
            if (serviceOrder.PaymentTransaction is not null)
            {
                serviceOrder.PaymentTransaction.Status = TransactionStatus.Failed;
                serviceOrder.PaymentTransaction.GatewayUsed = PaymentGateway.Fawry;
            }

            customerEmail = serviceOrder.Customer?.Email;
            orderDescription = $"خدمة رقم {serviceOrder.Id}";
        }
        else
        {
            jobOrder!.Status = OrderStatus.CancelledByClient;
            if (jobOrder.PaymentTransaction is not null)
            {
                jobOrder.PaymentTransaction.Status = TransactionStatus.Failed;
                jobOrder.PaymentTransaction.GatewayUsed = PaymentGateway.Fawry;
            }

            customerEmail = jobOrder.Customer?.Email;
            orderDescription = $"طلب عمل رقم {jobOrder.Id}";
        }

        await db.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(customerEmail))
        {
            var subject = "فشل عملية الدفع";
            var reason = string.IsNullOrWhiteSpace(errorMessage) ? "سبب غير محدد من بوابة الدفع." : errorMessage;
            var body = $"محاولة الدفع لـ {orderDescription} (فاتورة رقم {invoiceId}) فشلت.\n\nالسبب: {reason}";
            await emailHelper.SendEmailAsync(customerEmail, subject, body);
        }
    }

    public async Task HandlePaymentCancelledAsync(string referenceId)
    {
        if (string.IsNullOrWhiteSpace(referenceId))
            return;

        var payment = await db.PaymentTransactions
       .Include(p => p.ServiceOrder).ThenInclude(o => o!.Customer)
       .Include(p => p.JobOrder).ThenInclude(o => o!.Customer)
       .FirstOrDefaultAsync(p => p.GatewayReferenceId == referenceId);

        if (payment is null)
            return;

        payment.Status = TransactionStatus.Failed;
        string customerEmail,orderId;

       

        // ✅ الصح
        if (payment.ServiceOrder is not null)
        {   
            customerEmail = payment.ServiceOrder.Customer?.Email!;
            orderId = $"خدمة رقم {payment.ServiceOrder.Id}";

        }
        else if (payment.JobOrder is not null)
        {
            customerEmail = payment.JobOrder.Customer?.Email!;
            orderId = $"طلب عمل رقم {payment.JobOrder.Id}";

        }
        else
        {
            // لا يوجد طلب مرتبط، لا يمكن إرسال إشعار
            return;
        }

        await db.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(customerEmail))
        {
            //TODO: Fix the email content to be more informative and user-friendly
            var subject = "تم إلغاء عملية الدفع";
            var body = $"تم إلغاء عملية الدفع المرتبطة بالطلب رقم {orderId}. إذا لم تقم بهذا الإلغاء، يرجى التواصل مع الدعم.";
            await emailHelper.SendEmailAsync(customerEmail, subject, body);
        }
    }

    private async Task<(ServiceOrder? serviceOrder, JobOrder? jobOrder)> FindOrderByInvoiceAsync(long invoiceId, string invoiceKey)
    {
        var serviceOrder = await db.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.PaymentTransaction)
            .FirstOrDefaultAsync(o => o.InvoiceId == invoiceId && o.InvoiceKey == invoiceKey);

        if (serviceOrder is not null)
            return (serviceOrder, null);

        var jobOrders = db.Set<Job;
        var jobOrder = await jobOrders
            .Include(o => o.Customer)
            .Include(o => o.PaymentTransaction)
            .FirstOrDefaultAsync(o => o.InvoiceId == invoiceId && o.InvoiceKey == invoiceKey);

        return (null, jobOrder);
    }

    public async Task CompleteServiceOrderAsync(int orderId)
    {
        var order = await db.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.ServiceProviderProfile)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            return;

        if (order.Status != OrderStatus.Active)
            return;

        order.Status = OrderStatus.Completed;
        await db.SaveChangesAsync();

        var customerEmail = order.Customer?.Email;
        var providerEmail = order.ServiceProviderProfile?.User?.Email;

        var subject = "تم إكمال الطلب";
        var body = $"تم إكمال طلب الخدمة رقم {order.Id} بنجاح. شكرًا لتعاملكم عبر منصة خدماتك.";

        if (!string.IsNullOrWhiteSpace(customerEmail))
            await emailHelper.SendEmailAsync(customerEmail, subject, body);

        if (!string.IsNullOrWhiteSpace(providerEmail))
            await emailHelper.SendEmailAsync(providerEmail, subject, body);
    }

    public async Task OpenDisputeAsync(OrderDisputeRequest request, string currentUserId)
    {
        var order = await db.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.ServiceProviderProfile)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(o => o.Id == request.ServiceOrderId);

        if (order is null || order.Customer is null || order.ServiceProviderProfile?.User is null)
            return;

        // تحديد الرافع والمدعى عليه بناءً على IsRaiserCustomer
        var customer = order.Customer;
        var provider = order.ServiceProviderProfile.User;

        var raiser = request.IsRaiserCustomer ? customer : provider;
        var target = request.IsRaiserCustomer ? provider : customer;

        // تحديث حالة الطلب إلى Disputed
        order.Status = OrderStatus.Disputed;

        var dispute = new Dispute
        {
            ServiceOrderId = order.Id,
            RaiserId = raiser.Id,
            TargetId = target.Id,
            RaiserConversationId = request.RaiserConversationId,
            TargetConversationId = request.TargetConversationId,
            Status = DisputeStatus.Opened,
            Type = request.Type,
            AmountUnderDispute = request.AmountUnderDispute,
            ReasonDetails = request.ReasonDetails,
            OpenedDate = DateTime.UtcNow
        };

        db.Disputes.Add(dispute);
        await db.SaveChangesAsync();

        var subject = "تم فتح نزاع جديد على الطلب";
        var bodyForCustomer = $"تم فتح نزاع على طلب الخدمة رقم {order.Id}. سيتم التواصل معك من قبل فريق الدعم.";
        var bodyForProvider = $"تم فتح نزاع على طلب الخدمة رقم {order.Id} بينك وبين العميل. سيتم التواصل معك من قبل فريق الدعم.";

        if (!string.IsNullOrWhiteSpace(customer.Email))
            await emailHelper.SendEmailAsync(customer.Email, subject, bodyForCustomer);

        if (!string.IsNullOrWhiteSpace(provider.Email))
            await emailHelper.SendEmailAsync(provider.Email, subject, bodyForProvider);
    }
}
