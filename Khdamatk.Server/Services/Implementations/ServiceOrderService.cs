using Khdamatk.Server.Contracts.Conversations;
using Khdamatk.Server.Contracts.Service;
using Khdamatk.Server.Contracts.WebHook;
using Khdamatk.Server.Helper.Payment;
using Khdamatk.Server.Helper;
using Khdamatk.Server.Services.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Khdamatk.Server.Data.Entities.Catalog;
using Khdamatk.Server.Data.Entities.Interaction;
using Khdamatk.Server.Data.Entities.Operations;
using Microsoft.AspNetCore.Hosting;

namespace Khdamatk.Server.Services.Implementations;

public class ServiceOrderService(
    Database db,
    IFawaterakPaymentHelper fawaterak,
    IWebHostEnvironment env,
    IOptions<ClientSetting> options,
    IEmailHelper emailHelper,
    HybridCache cache
    ) : IServiceOrderService
{
    private readonly Database db = db;
    private readonly IFawaterakPaymentHelper fawaterak = fawaterak;
    private readonly IWebHostEnvironment env = env;
    private readonly ClientSetting clientSetting = options.Value;
    private readonly IEmailHelper emailHelper = emailHelper;
    private readonly HybridCache cache = cache;

    #region CRUD OPERATIONS FOR SERVICES

    public async Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken cancellationToken = default)
    {
        var category = await db.Categories.FirstOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);
        if (category == null)
        {
            category = new Category { Name = request.CategoryName };
            await db.Categories.AddAsync(category, cancellationToken);
            await db.SaveChangesAsync();
        }

        var mediaGalleryLinks = new List<ServiceMedia>();
        mediaGalleryLinks.AddRange(request.Attachment != null ? await Task.WhenAll(request.Attachment.Select(async file =>
        {
            var media = await file.UploadFileAsync();
            return new ServiceMedia {
                Media = media,
            };
        })) : new List<ServiceMedia>());

        var service = new Service
        {
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            DetailedDescription = request.DetailedDescription,
            Price = request.Price,
            DeliveryTimeInDays = request.DeliverTimeInDays,
            CategoryId = category.Id,
            ServiceProviderProfileId = request.ProviderProfileId,
            Concepts = request.Concepts,
            RevisionCount = request.RevisionCount,
            CreatedAt = DateTime.UtcNow,
            IsDelete = false,
            IsActive = true,
            AverageRating = 0,
            TotalReviews = 0,
            MainImage = await  request.ServiceEnvelope.UploadFileAsync()?? null,
            MediaGalleryLinks = mediaGalleryLinks ?? null
        };



        if (request.Attachment != null && request.Attachment.Count > 0)
        {
            service.MediaGalleryLinks = new List<ServiceMedia>();
            foreach (var item in request.Attachment)
            {
                var media = await item.UploadFileAsync();
                service.MediaGalleryLinks.Add(new ServiceMedia { Media = media });
            }
        }

        if (request.ServiceEnvelope != null)
        {
            service.MainImage = await request.ServiceEnvelope.UploadFileAsync();
        }

        await db.Services.AddAsync(service, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        
        await cache.RemoveByTagAsync("ServicesList", cancellationToken);

        return Success(StatusCodes.Status201Created, "Service created successfully.");
    }

    public async Task<resultBase> GetServiceAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        OrderServiceDetailsResponse? service = await cache.GetOrCreateAsync(
            $"Service_{serviceId}",
            async cancel => await db.Services
        .Where(s => s.Id == serviceId) // 1. الفلترة أولاً على مستوى الكيان الأساسي
        .ProjectToType<OrderServiceDetailsResponse>() // 2. الإسقاط إلى الـ DTO بعد الفلترة
        .FirstOrDefaultAsync(cancel),
            tags: [$"Service_{serviceId}", "ServicesList"]
        );

        if (service == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Service not found.");

        var response = service;

        return Success(StatusCodes.Status200OK, response);
    }

    public async Task<resultBase> GetServicesAsync(GetServicesRequest request, CancellationToken cancellationToken = default)
    {
        var query = db.Services
            .Include(s => s.Category)
            .Include(s => s.ServiceProviderProfile)
                .ThenInclude(p => p.User)
            .Where(s => !s.IsDelete)
            .AsNoTracking();

        if (request != null)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.Trim().ToLower();
                query = query.Where(s => s.Title.Contains(search) || (s.Concepts != null && s.Concepts.Contains(search)));
            }
            if (request.CategoryId.HasValue)
                query = query.Where(s => s.CategoryId == request.CategoryId);
            if (request.MinPrice.HasValue)
                query = query.Where(s => s.Price >= request.MinPrice);
            if (request.MaxPrice.HasValue)
                query = query.Where(s => s.Price <= request.MaxPrice);
        }

        var services = await query
            .Select(s => new ServiceSummaryResponse(
                s.Id,
                s.Title,
                s.ShortDescription,
                s.Price,
                s.ServiceProviderProfile.User.ProfilePicture != null ? System.IO.File.ReadAllBytes(s.ServiceProviderProfile.User.ProfilePicture.FullPath) : new byte[0],
                s.Orders.Count,
                s.AverageRating,
                s.DeliveryTimeInDays,
                new ProviderSummaryInfo(
                    s.ServiceProviderProfileId.ToString(),
                    s.ServiceProviderProfile.User.FullName!,
                    s.ServiceProviderProfile.User.ProfilePicture != null ? System.IO.File.ReadAllBytes(s.ServiceProviderProfile.User.ProfilePicture.FullPath) : new byte[0],
                    s.ServiceProviderProfile.AverageRating
                ),
                (s.ServiceProviderProfile.User.VerificationData != null)? $"{s.ServiceProviderProfile.User.VerificationData.Country},{s.ServiceProviderProfile.User.VerificationData.City}" : "N/A",
                s.IsActive,
                s.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return Success(StatusCodes.Status200OK, services);
    }

    public async Task<resultBase> UpdateServiceAsync(int serviceId, UpdateServiceRequest request, CancellationToken cancellationToken = default)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDelete, cancellationToken);

        if (service == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Service not found");

        service.Title = request.Title;
        service.ShortDescription = request.ShortDescription;
        service.DetailedDescription = request.DetailedDescription;
        service.Price = request.Price;
        service.DeliveryTimeInDays = request.DeliverTimeInDays;
        service.Concepts = request.Concepts;
        service.RevisionCount = request.RevisionCount;
        service.UpdatedAt = DateTime.UtcNow;

        db.Services.Update(service);
        await db.SaveChangesAsync(cancellationToken);
        
        await cache.RemoveByTagAsync($"Service_{serviceId}", cancellationToken);
        await cache.RemoveByTagAsync("ServicesList", cancellationToken);

        return Success(StatusCodes.Status200OK, "Service updated successfully.");
    }

    public async Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDelete, cancellationToken);

        if (service == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Service not found");

        service.IsDelete = true;
        await db.SaveChangesAsync(cancellationToken);

        await cache.RemoveByTagAsync($"Service_{serviceId}", cancellationToken);
        await cache.RemoveByTagAsync("ServicesList", cancellationToken);

        return Success(StatusCodes.Status200OK, "Service deleted successfully.");
    }

    #endregion

    #region Iniatal Order Operations

    public async Task<resultBase> AddOrderAsync(int serviceId, string customerId, OrderServiceRequest request, CancellationToken cancellationToken = default)
    {
        var service = await db.Services
            .Include(s => s.ServiceProviderProfile)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDelete, cancellationToken);

        if (service == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, "Service not found.");

        var customer = await db.Users.FindAsync([customerId], cancellationToken);
        if (customer == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, "Customer not found.");

        var order = new ServiceOrder
        {
            ServiceID = serviceId,
            CustomerId = customerId,
            ServiceProviderId = service.ServiceProviderProfileId,
            Amount = service.Price,
            Status = OrderStatus.PendingApproval,
            CreatedAt = DateTime.UtcNow,
            Conversation = new Conversation
            {
                CustomerId = customerId,
                ProviderId = service.ServiceProviderProfileId,
                Messages = new List<Message>()
            }
        };

        await db.ServiceOrders.AddAsync(order, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        order.Conversation.RelatedEntityId = order.Id;
        await db.SaveChangesAsync(cancellationToken);

        // Notify freelancer
        await emailHelper.SendJobPostConfirmationAsync(service.ServiceProviderProfile.User.Email!, service.ServiceProviderProfile.User.FullName!, service.Title);

        return Success(StatusCodes.Status201Created, "Order created", "The service order has been requested successfully.");
    }

    public async Task<resultBase> AcceptOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default)
    {
        var order = await db.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.Service)
            .Include(o => o.ServiceProviderProfile)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.ServiceProviderId == freelancerId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, "Order Not Found", "Order not found or unauthorized.");

        if (order.Status != OrderStatus.PendingApproval)
            return Failure(StatusCodes.Status400BadRequest, "Error", "Order cannot be accepted in its current state.");

        order.Status = OrderStatus.PendingPayment;
        await db.SaveChangesAsync(cancellationToken);

        await cache.RemoveByTagAsync($"Order_{orderId}", cancellationToken);

        // Send payment instruction email to customer
        await emailHelper.SendServiceAcceptanceAsync(order.Customer.Email!, order.Customer.FullName ?? order.Customer.UserName!, order.ServiceProviderProfile.User.FullName ?? order.ServiceProviderProfile.User.UserName!, order.Service.Title);

        return Success(StatusCodes.Status200OK, "Accepted", "You have accepted the order.");
    }

    public async Task<resultBase> RejectOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default)
    {
        var order = await db.ServiceOrders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.ServiceProviderId == freelancerId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, "Order Not Found", "Order not found or unauthorized.");

        order.Status = OrderStatus.Rejected;
        await db.SaveChangesAsync(cancellationToken);
        
        await cache.RemoveByTagAsync($"Order_{orderId}", cancellationToken);

        return Success(StatusCodes.Status200OK, "Rejected", "Order has been rejected.");
    }

    public async Task<resultBase> PayOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        ServiceOrder? order = await db.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.Service)
            .Include(o => o.ServiceProviderProfile)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);
        
        if (order.Status != OrderStatus.PendingPayment)
            return Failure(StatusCodes.Status400BadRequest, FailureMessages.General.Title, "Order must be in PendingPayment state to proceed with payment.");

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
            Status = order.Status,
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
                OnSuccess = clientSetting.ClientUrl.TrimEnd('/') + "/DashBoard?State=OnSuccess",
                OnFailure = clientSetting.ClientUrl.TrimEnd('/') + "/DashBoard?State=OnFailure",
                OnPending = clientSetting.ClientUrl.TrimEnd('/') + "/DashBoard?State=OnPending"
            }
        };

        var result = await fawaterak.CreateEInvoiceAsync(eInvoice);

        if (result != null)
        {
            order.InvoiceId = result.InvoiceId;
            order.InvoiceKey = result.InvoiceKey;

            await db.SaveChangesAsync(cancellationToken);

            return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
        }

        return Failure(StatusCodes.Status503ServiceUnavailable, new Error("Payment Gateway Error", "The Payment Service Not Available"));
    }

    public async Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken = default)
    {
        model.Payload = model.PayloadString != null ? JsonSerializer.Deserialize<InvoicePayload>(model.PayloadString) : null;

        if (model.Payload == null)
            return Failure(StatusCodes.Status400BadRequest, new Error("Invalid Payload", "The payload data is invalid or missing"));

        var order = await db.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.ServiceProviderProfile)
            .ThenInclude(p => p.User)
            .Include(o => o.Service)
            .FirstOrDefaultAsync(o => o.Id == model.Payload.OrderId && o.InvoiceKey == model.InvoiceKey, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, new Error("Order Not Found", "There are no order matching the provided details"));
        
        if (order.Status != OrderStatus.PendingPayment)
            return Failure(StatusCodes.Status400BadRequest, new Error("Invalid Order State", "The order is not in a pending payment state"));

        CurrencyCode CurrencyCode = CurrencyCode.EGP; 

        var platformFee = order.Amount * 0.1m; // 10% platform fee
        order.PaymentTransaction = new PaymentTransaction()
        {
            Amount = order.Amount,
            Currency = CurrencyCode,
            TransactionDate = DateTime.UtcNow,
            Status = TransactionStatus.Completed,
            NetPayout = order.Amount - platformFee,
            GatewayUsed = PaymentGateway.Card,
            PlatformFee = platformFee
        };

        order.Status = OrderStatus.Active;

        await db.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync($"Order_{order.Id}", cancellationToken);

        await emailHelper.SendJobInProgressAsync(order.Customer.Email!, order.Customer.FullName!, order.Service.Title);
        await emailHelper.SendJobInProgressAsync(order.ServiceProviderProfile.User.Email!, order.ServiceProviderProfile.User.FullName!, order.Service.Title);

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
    }

    public async Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken = default)
    {
        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, "Payment failure recorded.");
    }

    #endregion

    #region Core Order Operations

    public async Task<resultBase> GetOrderAsync(int orderId, string userId, CancellationToken cancellationToken = default)
    {
        var order = await cache.GetOrCreateAsync(
            $"Order_{orderId}_{userId}",
            async cancel => await db.ServiceOrders
                .Include(o => o.Service)
                .Include(o => o.Customer)
                .Include(o => o.ServiceProviderProfile)
                .ThenInclude(p => p.User)
                .Where(o => o.Id == orderId && (o.CustomerId == userId || o.ServiceProviderId == userId))
                .Select(o => new OrderResponse(
                    o.Id,
                    o.Service.Title,
                    o.Service.ShortDescription,
                    o.Amount,
                    o.Status.ToString(),
                    o.CreatedAt,
                    o.ServiceProviderProfile.User.UserName!,
                    o.Customer.UserName!
                )).FirstOrDefaultAsync(cancel),
            tags: [$"Order_{orderId}"]
        );

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Order not found");

        return Success(StatusCodes.Status200OK, order);
    }
    
    public async Task<resultBase> GetOrdersAsync(string userId, CancellationToken cancellationToken = default)
    {
        var orders = await db.ServiceOrders
            .Where(o => o.CustomerId == userId || o.ServiceProviderId == userId)
            .OrderByDescending(o => o.Id)
            .Select(o => new OrderResponse(
                o.Id,
                o.Service.Title,
                o.Service.ShortDescription,
                o.Amount,
                o.Status.ToString(),
                o.CreatedAt,
                o.ServiceProviderProfile.User.UserName!,
                o.Customer.UserName!
            ))
            .ToListAsync(cancellationToken);

        return Success(StatusCodes.Status200OK, orders);
    }
    
    public async Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default)
    {
        var order = await db.ServiceOrders
            .Include(o => o.Conversation)
            .ThenInclude(c => c.Messages)
            .Include(o => o.Customer)
            .Include(o => o.ServiceProviderProfile)
            .ThenInclude(p => p.User)
            .Include(o => o.Service)
            .FirstOrDefaultAsync(o => o.Id == orderId && (o.CustomerId == userId || o.ServiceProviderId == userId), cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            if (order.MediaAttachments == null) order.MediaAttachments = new List<Media>();
            
            foreach (var file in request.Attachments)
            {
                var media = await file.UploadFileAsync();
                order.MediaAttachments.Add(media);
            }
        }

        if (order.Conversation != null)
        {
            order.Conversation.Messages.Add(new()
            {
                SenderId = userId,
                Content = request.Message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync($"Order_{orderId}", cancellationToken);

        await emailHelper.SendWorkDeliveryAsync(order.Customer.Email!, order.Customer.FullName ?? order.Customer.UserName!);
        await emailHelper.SendWorkDeliveryAsync(order.ServiceProviderProfile.User.Email!, order.ServiceProviderProfile.User.FullName ?? order.ServiceProviderProfile.User.UserName!);

        return await GetOrderAsync(orderId, userId, cancellationToken);
    }

    public async Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default)
    {
        var conversations = await db.ServiceOrders
            .Where(c => c.CustomerId == userId || c.ServiceProviderId == userId)
            .Select(c => new ConversationsSummaryResponse(
                userId == c.CustomerId ? c.ServiceProviderId : c.CustomerId,
                (userId == c.CustomerId) ? c.ServiceProviderProfile.User.UserName! : c.Customer.UserName!,
                (userId == c.CustomerId) ? (c.ServiceProviderProfile.User.ProfilePicture != null ? c.ServiceProviderProfile.User.ProfilePicture.FullPath : "") : (c.Customer.ProfilePicture != null ? c.Customer.ProfilePicture.FullPath : ""),
                c.Service.Title,
                c.Conversation!.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()!.Content : "",
                c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()!.CreatedAt : DateTime.MinValue,
                c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()!.IsRead : true,
                c.Id
            ))
            .ToListAsync(cancellationToken);

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, conversations);
    }

    public async Task<resultBase> GetConversationMessages(int orderId, string userId, CancellationToken cancellationToken = default)
    {
        var order = await db.ServiceOrders
            .Include(o => o.Conversation)
            .ThenInclude(c => c.Messages)
            .Include(o => o.Service)
            .Include(o => o.Customer)
            .ThenInclude(u => u.ProfilePicture)
            .Include(o => o.ServiceProviderProfile)
            .ThenInclude(p => p.User)
            .ThenInclude(u => u.ProfilePicture)
            .FirstOrDefaultAsync(o => o.Id == orderId && (o.CustomerId == userId || o.ServiceProviderId == userId), cancellationToken);

        if (order == null || order.Conversation == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        var converationDetailed = new ConversationsDetailed(
            order.Conversation.Id,
            order.Service.Title,
            userId,
            (userId == order.CustomerId) ? order.Customer.UserName! : order.ServiceProviderProfile.User.UserName!,
            (userId == order.CustomerId) ? (order.Customer.ProfilePicture != null ? order.Customer.ProfilePicture.DownloadFileAsyncPathVersion() : "") : (order.ServiceProviderProfile.User.ProfilePicture != null ? order.ServiceProviderProfile.User.ProfilePicture.DownloadFileAsyncPathVersion() : ""),
            (userId != order.CustomerId) ? order.CustomerId : order.ServiceProviderId,
            (userId != order.CustomerId) ? order.Customer.UserName! : order.ServiceProviderProfile.User.UserName!,
            (userId != order.CustomerId) ? (order.Customer.ProfilePicture != null ? order.Customer.ProfilePicture.DownloadFileAsyncPathVersion() : "") : (order.ServiceProviderProfile.User.ProfilePicture != null ? order.ServiceProviderProfile.User.ProfilePicture.DownloadFileAsyncPathVersion() : ""),
            order.Conversation.Messages.Select(m => new ConversationMessageResponse(m.Id, m.Content, m.SenderId, m.CreatedAt)).ToList()
        );

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, converationDetailed);
    }

    #endregion

    #region Final Order Operations

    public async Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken cancellationToken = default)
    {
        var order = await db.ServiceOrders
            .Include(o => o.ServiceProviderProfile)
            .ThenInclude(p => p.User)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if (order.Status != OrderStatus.Active && order.Status != OrderStatus.UnderReview)
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, "Order must be active or under review to be completed.");

        order.ServiceProviderProfile.User.Amount += order.Amount;

        order.Review = new Khdamatk.Server.Data.Entities.Interaction.Review()
        {
            Rating = request.Rating,
            Content = request.Content,
            Title = request.Title,
            ReviewerId = order.CustomerId,
            ServiceProviderId = order.ServiceProviderId
        };

        order.Status = OrderStatus.Completed;

        await db.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync($"Order_{orderId}", cancellationToken);

        await emailHelper.SendJobCompletedAsync(order.ServiceProviderProfile.User.Email!, order.ServiceProviderProfile.User.FullName!, "Service Order Completed");

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
    }

    public async Task<resultBase> CancelOrderAsync(int orderId, string userId, CancellationToken cancellationToken = default)
    {
        var order = await db.ServiceOrders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if (order.CustomerId != userId && order.ServiceProviderId != userId)
            return Failure(StatusCodes.Status403Forbidden, FailureMessages.Forbidden.Title, FailureMessages.Forbidden.Message);

        var wasActive = order.Status == OrderStatus.Active;

        if (order.CustomerId == userId)
        {
            order.Status = OrderStatus.CancelledByClient;
        }
        else if (order.ServiceProviderId == userId)
        {
            order.Status = OrderStatus.CancelledByProvider;
        }

        if (wasActive && order.CustomerId == userId)
        {
            order.Customer.Amount += order.Amount;
        }

        await db.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync($"Order_{orderId}", cancellationToken);

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, "Order cancelled successfully.");
    }
    
    public async Task<resultBase> OpenDispute(int orderId, string raiserId, string reasonDetails, CancellationToken cancellationToken = default)
    {
        ServiceOrder? order = await db.ServiceOrders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if (order.Status != OrderStatus.Active)
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, "Order must be active to open a dispute.");

        if (order.CustomerId != raiserId && order.ServiceProviderId != raiserId)
            return Failure(StatusCodes.Status403Forbidden, FailureMessages.Forbidden.Title, FailureMessages.Forbidden.Message);

        if (await db.Disputes.AnyAsync(o => o.JobOrderId == orderId, cancellationToken))
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, "Dispute already exists.");

        order.Status = OrderStatus.Disputed;
        order.Dispute = new()
        {
            JobOrderId = orderId, 
            RaiserId = raiserId,
            ReasonDetails = reasonDetails
        };

        await db.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync($"Order_{orderId}", cancellationToken);

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, "Dispute opened.");
    }

    #endregion
}
