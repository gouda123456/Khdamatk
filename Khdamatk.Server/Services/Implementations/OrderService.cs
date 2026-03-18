using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.Orders;
using Khdamatk.Server.Helper.Payment;

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

    public async Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> StartServiceOrderPaymentAsync(int orderId)
    {
        var order = await db.ServiceOrders
            .Include(o => o.User)
            .Include(o => o.Service)
            .Include(o => o.ServiceProviderProfile)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null || order.User is null || order.Service is null || order.ServiceProviderProfile is null)
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
                FirstName = order.User.UserName ?? string.Empty,
                LastName = string.Empty,
                CustomerId = order.User.Id,
                Email = order.User.Email ?? string.Empty
            },
            CartItems = new List<CartItemModel>
            {
                new CartItemModel
                {
                    Name = order.Service.Title,
                    Quantity = 1,
                    Price = order.Amount
                }
            },
            PayLoad = new InvoicePayload
            {
                OrderId = order.Id,
                OrderType = OrderType.Service,
                Provider = new ProviderModel
                {
                    Id = order.ServiceProviderProfile.UserId,
                    Username = order.ServiceProviderProfile.User.UserName ?? string.Empty,
                    Email = order.ServiceProviderProfile.User.Email ?? string.Empty
                }
            },
            Status = OrderStatus.PendingPayment
        };

        var response = await fawaterakPaymentHelper.CreateEInvoiceAsync(request);
        if (response is null)
            return null;

        // إنشاء أو تحديث معاملة الدفع المرتبطة بالطلب
        var transaction = order.PaymentTransaction;
        if (transaction is null)
        {
            transaction = new PaymentTransaction
            {
                ServiceOrder = order,
                ServiceOrderId = order.Id,
            };
            db.PaymentTransactions.Add(transaction);
            order.PaymentTransaction = transaction;
        }

        transaction.Amount = order.Amount;
        transaction.PlatformFee = Math.Round(order.Amount * 0.10m, 2);
        transaction.NetPayout = transaction.Amount - transaction.PlatformFee;
        transaction.Currency = CurrencyCode.EGP;
        transaction.Status = TransactionStatus.Pending;
        transaction.GatewayUsed = PaymentGateway.Fawry;

        if (long.TryParse(response.InvoiceId, out var invoiceId))
        {
            order.InvoiceId = invoiceId;
            order.InvoiceKey = response.InvoiceKey;
        }

        await db.SaveChangesAsync();

        return response;
    }

    public async Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> StartJobOrderPaymentAsync(int jobOrderId)
    {
        var jobOrderSet = db.Set<JobOrder>();

        var order = await jobOrderSet
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

        var response = await fawaterakPaymentHelper.CreateEInvoiceAsync(request);
        if (response is null)
            return null;

        if (long.TryParse(response.InvoiceId, out var invoiceId))
        {
            order.InvoiceId = invoiceId;
            order.InvoiceKey = response.InvoiceKey;
        }

        await db.SaveChangesAsync();

        return response;
    }

    public async Task HandlePaymentSuccessAsync(long invoiceId, string invoiceKey)
    {
        var (serviceOrder, jobOrder) = await FindOrderByInvoiceAsync(invoiceId, invoiceKey);
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

            customerEmail = serviceOrder.User?.Email;
            orderDescription = $"خدمة رقم {serviceOrder.Id}";
        }
        else
        {
            jobOrder!.Status = OrderStatus.Active;
            if (jobOrder.PaymentTransaction is not null)
            {
                jobOrder.PaymentTransaction.Status = TransactionStatus.Completed;
                jobOrder.PaymentTransaction.GatewayUsed = PaymentGateway.Fawry;
            }

            customerEmail = jobOrder.Customer?.Email;
            orderDescription = $"طلب عمل رقم {jobOrder.Id}";
        }

        await db.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(customerEmail))
        {
            var subject = "تم الدفع بنجاح";
            var body = $"تم إتمام عملية الدفع بنجاح لـ {orderDescription} (فاتورة رقم {invoiceId}). شكرًا لاستخدامك خدماتك.";
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

            customerEmail = serviceOrder.User?.Email;
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
       .Include(p => p.ServiceOrder).ThenInclude(o => o!.User)
       .Include(p => p.JobOrder).ThenInclude(o => o!.Customer)
       .FirstOrDefaultAsync(p => p.GatewayReferenceId == referenceId);

        if (payment is null)
            return;

        payment.Status = TransactionStatus.Failed;
        string customerEmail,orderId;

       

        // ✅ الصح
        if (payment.ServiceOrder is not null)
        {   
            customerEmail = payment.ServiceOrder.User?.Email!;
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
            .Include(o => o.User)
            .Include(o => o.PaymentTransaction)
            .FirstOrDefaultAsync(o => o.InvoiceId == invoiceId && o.InvoiceKey == invoiceKey);

        if (serviceOrder is not null)
            return (serviceOrder, null);

        var jobOrders = db.Set<JobOrder>();
        var jobOrder = await jobOrders
            .Include(o => o.Customer)
            .Include(o => o.PaymentTransaction)
            .FirstOrDefaultAsync(o => o.InvoiceId == invoiceId && o.InvoiceKey == invoiceKey);

        return (null, jobOrder);
    }

    public async Task CompleteServiceOrderAsync(int orderId)
    {
        var order = await db.ServiceOrders
            .Include(o => o.User)
            .Include(o => o.ServiceProviderProfile)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            return;

        if (order.Status != OrderStatus.Active)
            return;

        order.Status = OrderStatus.Completed;
        await db.SaveChangesAsync();

        var customerEmail = order.User?.Email;
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
            .Include(o => o.User)
            .Include(o => o.ServiceProviderProfile)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(o => o.Id == request.ServiceOrderId);

        if (order is null || order.User is null || order.ServiceProviderProfile?.User is null)
            return;

        // تحديد الرافع والمدعى عليه بناءً على IsRaiserCustomer
        var customer = order.User;
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


    }
}
