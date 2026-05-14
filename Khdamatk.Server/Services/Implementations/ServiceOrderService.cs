using System.Text.Json;
using Khdamatk.Server.Contracts.Conversations;
using Khdamatk.Server.Contracts.Service;
using Khdamatk.Server.Contracts.WebHook;
using Khdamatk.Server.Helper.Payment;
using Microsoft.Identity.Client;
using Stripe;

namespace Khdamatk.Server.Services.Implementations;

public class ServiceOrderService(Database db,IFawaterakPaymentHelper fawaterak) : IServiceOrderService
{
    private readonly Database db = db;
    private readonly IMapper _mapper;

    private readonly IFawaterakPaymentHelper fawaterak = fawaterak;



    #region CRUD OPERATIONS FOR SERVICES

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


    #endregion



    #region Iniatal Order Operations

    public async Task<resultBase> AddOrderAsync(int ServiceId, string CustomerId, OrderServiceRequest request, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check if service is exists
         * check if customer is exists
         * create order with pendingApproval state
         * create conversation for this order
         * send Email to freelancer about new order
         * return order details to customer
         */

        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> FreeLancerAcceptOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        //TODO: change order state from pendingApproval to PendingPayment
        //TODO: send email to customer with payment link (using Flatwaterk)

        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> FreelancerRejectOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> PayOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check if order is exists and state == pendingPayment        
         * send E Invoice to client with payment link (using Flatwaterk)
         */

        ServiceOrder? order = await db.ServiceOrders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken: cancellationToken);

        if(order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);
        if(order.Status != OrderStatus.PendingPayment)
            return Failure(StatusCodes.Status400BadRequest, FailureMessages.General.Title, "Order must be in accepted state to proceed with payment.");

        EInvoiceRequestModel eInvoice = new EInvoiceRequestModel
        {
            SendEmail = true,
            Customer = new CustomerModel
            {
                 CustomerId = order.CustomerId,
                 FirstName = order.Customer!.UserName!,
                 LastName = "",
                 Email = order.Customer!.Email!,
                 Phone = order.Customer!.PhoneNumber!,
            },
            CartItems = new()
            {
                new CartItemModel()
                {
                    Name = order.Service.Title,
                    Quantity = 1,
                    Price = order.Service.Price
                }
            },
            Currency = CurrencyCode.EGP.ToString(),
            DueDate = DateTime.UtcNow.AddDays(7),
            PayLoad = new InvoicePayload()
            {
                OrderId = order.Id,
                OrderType = OrderType.Service,
                Provider = new ProviderModel()
                {
                    Id = order.ServiceProviderId,
                    Username = order.ServiceProviderProfile.User.UserName!,
                    Email = order.ServiceProviderProfile.User.Email!
                },
            },
            RedirectionUrls = new()
            {
                OnFailure = "https://www.youtube.com/",
                OnPending = "https://www.youtube.com/",
                OnSuccess = "https://www.youtube.com/"
            }
        };

        var result = await fawaterak.CreateEInvoiceAsync(eInvoice);

        if (result != null)
        {
            order.InvoiceId = result.InvoiceId;
            order.InvoiceKey = result.InvoiceKey;

            await db.SaveChangesAsync(cancellationToken);

            //TODO: send email to customer 
            //TODO: send email to freelancer

            return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
        }

        //TODO: send email to customer (Failed)

        return Failure(StatusCodes.Status503ServiceUnavailable, new Error("Payment Gateway Error", "The Payment Service Not Available"));

    }

    public async Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * Deserialize payload to get order details                 --Done
         * check if order is exists and state == pendingPayment     --Done
         */

        //TODOs: change order state from pendingPayment to Active
        //TODO: send email to freelancer about new active order


        model.Payload = model.PayloadString != null ? JsonSerializer.Deserialize<InvoicePayload>(model.PayloadString) : null;

        if (model.Payload != null)
        {
            return Failure(StatusCodes.Status400BadRequest, new Error("Invalid Payload", "The payload data is invalid or missing"));
        }

        var order = await db.JobOrders.FirstOrDefaultAsync(o => o.Id == model.Payload!.OrderId && o.InvoiceKey == model.InvoiceKey, cancellationToken: cancellationToken);

        if (order == null)
        {
            return Failure(StatusCodes.Status404NotFound, new Error("Order Not Found", "There are no order matching the provided details"));
        }
        
        if (order.Status != OrderStatus.PendingPayment)
        {
            return Failure(StatusCodes.Status400BadRequest, new Error("Invalid Order State", "The order is not in a pending payment state"));
        }

        CurrencyCode CurrencyCode = CurrencyCode.EGP;  //TODO: get currency code from model or order



        order.PaymentTransaction = new PaymentTransaction()
        {
            Amount = order.Amount,
            Currency = CurrencyCode,
            TransactionDate = DateTime.UtcNow,
            Status = TransactionStatus.Completed,
            NetPayout = order.Amount,
            GatewayUsed = PaymentGateway.Card,
            PlatformFee = order.Amount * 0.1m // Assuming a 10% platform fee
        };

        order.Status = OrderStatus.Active;

        await db.SaveChangesAsync(cancellationToken);

        //TODO: send email to Customer
        //TODO: send email to Free Lancer

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);

        
    }
    public async Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken = default)
    {
        //TODOs: send email to customer about payment failure and instructions to retry payment

        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }


    #endregion



    #region Core Order Operations

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
        /*TODOs:
         * check if order is exists and state == in progress        --Done
         * Validate request             --Done
         * check if there any attachments           --Done
         * check the user id to determine who submit the work (customer , freelancer)      --Done
         * add to conversation              --Done
         * convert List<IFormFile> to Media                 ****
         * (Feature:IFormFile.ToMedia(),list<IFormFile>): (params IFormFile[] Medias) => {store Data in project + convert IFormFile to media entity}
         */

        var Joborder = await db.ServiceOrders.FirstOrDefaultAsync(o => o.Id == orderId && (o.CustomerId == userId || o.ServiceProviderId == userId));


        if (Joborder == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            //TODO: convert List<IFormFile> to List<Media> then save it in DB with relation to order
            //TODO: store files in project (wwwroot/Uploads/JobOrderId/)

        }

        Joborder.Conversation.Messages.Add(new()
        {
            SenderId = userId,
            Content = request.Message,
            IsRead = false,
        });

        await db.SaveChangesAsync(cancellationToken);

        return await GetOrderAsync(orderId, cancellationToken);


    }

    public async Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default)
    {
        var conversations = await db.ServiceOrders.Where(c => c.CustomerId == userId || c.ServiceProviderId == userId).
            Select(c => new ConversationsSummaryResponse(
                userId,
        (userId == c.CustomerId) ? c.Customer.UserName : c.ServiceProviderProfile.User.UserName,
        (userId == c.CustomerId) ? c.Customer.ProfilePicture.FullPath : c.ServiceProviderProfile.User.ProfilePicture.FullPath,
        c.Service.Title,
        c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()!.Content : "",
                c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()!.CreatedAt : DateTime.MinValue,
                c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()!.IsRead : true
                 )
                ).ToListAsync(cancellationToken: cancellationToken);


        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, conversations);
    }
    public async Task<resultBase> GetConversationMessages(int orderId, string UserId, CancellationToken cancellationToken = default)
    {
        var order = await db.ServiceOrders.FirstOrDefaultAsync(o => o.Id == orderId && (o.CustomerId == UserId || o.ServiceProviderId == UserId), cancellationToken: cancellationToken);

        var converationDetailed = new ConversationsDetailed(
            order.Conversation.Id,
            order.Service.Title,
            UserId,
            (UserId == order.CustomerId) ? order.Customer.UserName : order.ServiceProviderProfile.User.UserName,
            (UserId == order.CustomerId) ? order.Customer.ProfilePicture.DownloadFileAsyncPathVersion() : order.ServiceProviderProfile.User.ProfilePicture.DownloadFileAsyncPathVersion(),
            (UserId != order.CustomerId) ? order.CustomerId : order.ServiceProviderId,
            (UserId != order.CustomerId) ? order.Customer.UserName : order.ServiceProviderProfile.User.UserName,
            (UserId != order.CustomerId) ? order.Customer.ProfilePicture.DownloadFileAsyncPathVersion() : order.ServiceProviderProfile.User.ProfilePicture.DownloadFileAsyncPathVersion(),
            order.Conversation.Messages.Select(m => new ConversationMessageResponse(m.Id, m.Content, m.SenderId, m.CreatedAt)).ToList()
            );


        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, converationDetailed);
    }

    #endregion


    #region Final Order Operations

    public async Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check if order.state == in progress      --Done
         * add offer Amount to free lancer          --Done (without refund process)
         * add review to order            --Done
         * order.state = complete           --Done
         * send email to free lancer
         */

        var order = await db.ServiceOrders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if (order.Status != OrderStatus.Active)
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, FailureMessages.Conflict.Message);

        var freelancer = await db.ServiceProviderProfiles.FirstOrDefaultAsync(s => s.UserId == order.ServiceProviderId, cancellationToken);

        //TODO: Add endpoint to pay for user Amount + refunds + checkout (pull money) )    --Not Done yet

        freelancer!.User.Amount += order.Amount;

        order.Review = new Data.Entities.Interaction.Review()
        {
            Rating = request.Rating,
            Content = request.Content,
            Title = request.Title,
            ReviewerId = order.CustomerId,
            ServiceProviderId = order.ServiceProviderId
        };

        order.Status = OrderStatus.Completed;

        await db.SaveChangesAsync(cancellationToken);

        //TODO: send email to free lancer

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
    }

    public async Task<resultBase> CancelOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> OpenDispute(int orderId, string RaiserId, string ReasonDetails, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check if order.state == in progress          --Done
         * compare the RaiserId to know if it customer or freelancer            --Done
         * create Dispute object            --Done
         * send email to the 3 party (customer , freelancer , admins)               ***
         */

        ServiceOrder? order = await db.ServiceOrders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken = default);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if (order.Status != OrderStatus.Active)
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, FailureMessages.Conflict.Message);

        if (order.CustomerId != RaiserId && order.ServiceProviderId != RaiserId)
            return Failure(StatusCodes.Status403Forbidden, FailureMessages.Forbidden.Title, FailureMessages.Forbidden.Message);

        if (db.Disputes.Any(o => o.JobOrderId == orderId))
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, FailureMessages.Conflict.Message);


        if (order.CustomerId == RaiserId)
        {
            order.Status = OrderStatus.Disputed;
            order.Dispute = new()
            {
                JobOrderId = orderId,
                RaiserId = RaiserId,
                ReasonDetails = ReasonDetails
            };
        }



        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
    }

    #endregion

    // 1. إضافة طلب جديد (بناءً على شغلانة وعرض سعر مقبول)
    public async Task<resultBase> AddOrderAsync1(CreateJobOrderRequest request, string customerId, CancellationToken cancellationToken = default)
    {
        // نجيب الشغلانة والعرض عشان نستخدمهم في الـ BuildOrder
        var job = await db.JobPosts.FindAsync([request.JobPostId], cancellationToken);
        var offer = await db.JobOffers.FindAsync([request.OfferId], cancellationToken);

        if (job == null || offer == null)
            return Failure(StatusCodes.Status404NotFound, "Data Not Found", "Job or Offer not found.");

        // استخدام الـ Static Method اللي إنت عاملها في الكلاس
        var order = JobOrder.BuildOrder(job, offer);

        await db.JobOrders.AddAsync(order, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return Success(StatusCodes.Status201Created, "Order Created", "The job order has been created successfully.");
    }

    // 2. قبول الأوردر من طرف الفريلانسر
    public async Task<resultBase> AcceptOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default)
    {
        // لاحظ استخدام ServiceProviderId بدل ProviderId
        var order = await db.JobOrders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.ServiceProviderId == freelancerId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, "Order Not Found", "Order not found or unauthorized.");

        // التأكد إن الـ Status نوعها Enum
        if (order.Status != OrderStatus.Pending)
            return Failure(StatusCodes.Status400BadRequest, "Error", "Order is already processed.");

        order.Status = OrderStatus.Accepted; // اتأكد إن Accepted موجودة في الـ Enum
        await db.SaveChangesAsync(cancellationToken);

        return Success(StatusCodes.Status200OK, "Accepted", "You have accepted the order.");
    }

    // 3. رفض الأوردر من طرف الفريلانسر
    public async Task<resultBase> RejectOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default)
    {
        var order = await db.JobOrders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.ServiceProviderId == freelancerId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, "Order Not Found", "Order not found.");

        order.Status = OrderStatus.Rejected; // اتأكد إن Rejected موجودة في الـ Enum
        await db.SaveChangesAsync(cancellationToken);

        return Success(StatusCodes.Status200OK, "Rejected", "Order has been rejected.");
    }
    // 1. تجيب أوردر واحد محدد
    public async Task<resultBase> GetOrderById(int id, string userId)
    {
        var order = await db.JobOrders
    .Where(o => o.Id == id && (o.CustomerId == userId || o.ServiceProviderId == userId))
    .Select(o => new OrderResponse(
        o.Id,
        o.Job.Title,             // ✅ صح: بنجيبها من جدول الـ Job المرتبط
        o.Job.Description,       // ✅ صح: بنجيبها من جدول الـ Job المرتبط
        o.Amount,                // ✅ صح: في الـ Entity عندك اسمها Amount
        o.Status.ToString(),
        o.CreatedAt,             // تأكد إن الاسم ده موجود في OrderBase
        o.ServiceProviderProfile.User.UserName, // ✅ صح: الاسم اللي في الـ Entity
        o.Customer.UserName      // ✅ صح: اسم العميل
    ))
    .FirstOrDefaultAsync();

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Order not found");

        return Success(StatusCodes.Status200OK, order);
    }

    // 2. تجيب كل أوردرات المستخدم
    public async Task<resultBase> GetUserOrders(string userId)
    {
        var orders = await db.JobOrders
            .Where(o => o.CustomerId == userId || o.ServiceProviderId == userId)
            .OrderByDescending(o => o.Id) // ترتيب حسب الأحدث
            .Select(o => new OrderResponse(
                o.Id,
                o.Job.Title,
                o.Job.Description,
                o.Amount,
                o.Status.ToString(),
                o.CreatedAt,
                o.ServiceProviderProfile.User.UserName,
                o.Customer.UserName
            ))
            .ToListAsync();

        return Success(StatusCodes.Status200OK, orders);
    }

    ///////////s////////


    // 1. الحصول على قائمة الخدمات مع البحث والفلترة (GetServices)
    public async Task<resultBase> GetServices(GetServicesRequest request, CancellationToken ct)
    {
        // بناء الاستعلام الأساسي (IQueryable) لضمان الأداء
        var query = db.Services
            .Include(s => s.Category)
            .Include(s => s.ServiceProviderProfile)
                .ThenInclude(p => p.User) // للوصول لاسم الشخص من جدول AspNetUsers
            .Where(s => !s.IsDelete)
            .AsNoTracking() // تحسين أداء القراءة
            .AsQueryable();

        // منطق البحث: لو اليوزر بعت SearchTerm، بيبحث في العنوان والـ Concepts
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim().ToLower();
            query = query.Where(s => s.Title.Contains(search)
                      || (s.Concepts != null && s.Concepts.Contains(search)));
        }

        // منطق الفلترة (اختياري حسب الـ Request)
        if (request.CategoryId.HasValue)
            query = query.Where(s => s.CategoryId == request.CategoryId);

        if (request.MinPrice.HasValue)
            query = query.Where(s => s.Price >= request.MinPrice);

        if (request.MaxPrice.HasValue)
            query = query.Where(s => s.Price <= request.MaxPrice);

        // تنفيذ الاستعلام وتحويله لـ List من الـ Response Contract المختصر
        var services = await query
            .Select(s => new ServiceSummaryResponse(
                s.Id,
                s.Title,
                s.ShortDescription,
                s.Price,
                s.AverageRating,
                s.TotalReviews,
                s.Category.Name,
                s.ServiceProviderProfile.User.FullName
            ))
            .ToListAsync(ct);

        return Success(StatusCodes.Status200OK, services);
    }

    // 2. الحصول على خدمة واحدة بكامل تفاصيلها (GetService)
    public async Task<resultBase> GetServiceById(int id, CancellationToken ct)
    {
        var service = await db.Services
            .Include(s => s.Category)
            .Include(s => s.ServiceProviderProfile)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete, ct);

        if (service == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Service not found.");
        var response = _mapper.Map<OrderServiceDetailsResponse>(service);
        return Success(StatusCodes.Status200OK, response);
    }

    // 3. إضافة خدمة جديدة (AddService) - Manual Mapping
    public async Task<resultBase> AddService(AddServiceRequest1 request, CancellationToken ct)
    {
        // بنحول الـ Request لـ Entity يدويًا عشان نتجنب الـ Null Mapper
        var service = new Khdamatk.Server.Data.Entities.Catalog.Service
        {
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            DetailedDescription = request.DetailedDescription,
            Price = request.Price,
            DeliveryTimeInDays = request.DeliveryTimeInDays,
            CategoryId = request.CategoryId,
            ServiceProviderProfileId = request.ServiceProviderProfileId,

            // تعيين القيم التلقائية
            CreatedAt = DateTime.UtcNow,
            IsDelete = false,
            //IsActive = true, // مهمة جدًا عشان الأيرور اللي ظهرلك في الداتابيز
            AverageRating = 0,
            TotalReviews = 0
        };

        try
        {
            await db.Services.AddAsync(service, ct);
            await db.SaveChangesAsync(ct); // دي اللحظة اللي كنت واقف عندها في الـ Debugger

            return Success(StatusCodes.Status201Created, "Service created successfully.");
        }
        catch (DbUpdateException)
        {
            // لو حصل تعارض في الـ Foreign Key (زي ما حصل معاك قبل كدة)
            return Failure(StatusCodes.Status400BadRequest, "error", "Service not found");
        }
    }

    // 4. تحديث خدمة موجودة (UpdateService)
    public async Task<resultBase> UpdateService(int id, UpdateServiceRequest request, CancellationToken ct)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete, ct);

        if (service == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Order not found");
        // تحديث البيانات من الـ Request
        _mapper.Map(request, service);
        service.UpdatedAt = DateTime.UtcNow;

        db.Services.Update(service);
        await db.SaveChangesAsync(ct);

        return Failure(StatusCodes.Status404NotFound, "Error", "Order not found");
    }

    // 5. حذف خدمة (DeleteService - Soft Delete)
    public async Task<resultBase> DeleteService(int id, CancellationToken ct)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete, ct);

        if (service == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Order not found");

        service.IsDelete = true; // Soft Delete بدلاً من المسح النهائي
        await db.SaveChangesAsync(ct);

        return Failure(StatusCodes.Status404NotFound, "Error", "Order not found");
    }
}
