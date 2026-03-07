using Hangfire.Common;
using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Helper.Payment;

namespace Khdamatk.Server.Services.Implementations;

public class JobOrderService(Database db ,IFawaterakPaymentHelper fawaterak) : IJobOrderService
{
    private readonly Database db = db;
    private readonly IFawaterakPaymentHelper fawaterak = fawaterak;

    public async Task<resultBase> AcceptOfferJob(int jobId, int offerId, string CustomerId, CancellationToken cancellationToken)
    {

        var offer = db.JobOffers.Find(offerId);
        if (offer == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.NotFound.Title, FailureMessages.NotFound.Message);

        var Job = db.JobPosts.Find(jobId);
        if (Job == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.NotFound.Title, FailureMessages.NotFound.Message);

        if (offer.JobPostId != jobId)
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, FailureMessages.Conflict.Message);


        // ✅ Authorization
        if (Job.CustomerId != CustomerId)
            return Failure(StatusCodes.Status403Forbidden,
                FailureMessages.Forbidden.Title, FailureMessages.Forbidden.Message);

        // ✅ Status Checks
        if (Job.Status != JobPostStatus.Open)
            return Failure(StatusCodes.Status409Conflict,
                FailureMessages.Conflict.Title, FailureMessages.Conflict.Message);

        if (offer.Status == JobOfferStatus.Accepted)
            return Failure(StatusCodes.Status409Conflict,
                FailureMessages.Conflict.Title, FailureMessages.Conflict.Message);

        // ✅ Update Statuses
        offer.Status = JobOfferStatus.Accepted;
        Job.Status = JobPostStatus.Closed;


        var order = new JobOrder().BuildOrder(Job, offer);


        await db.JobOrders.AddAsync(order);
        await db.SaveChangesAsync(cancellationToken);

        order.Conversation.RelatedEntityId = order.Id;
        await db.SaveChangesAsync(cancellationToken);

        var eInvoiceRequestModel = new EInvoiceRequestModel()
        {
            CartItems =
            [
                new CartItemModel()
                {
                    Name = Job.Title,
                    Price=offer.ProposedPrice,
                    Quantity=1
                }
                ],
            Currency = "EGP",
            Customer = new CustomerModel()
            {
                CustomerId = CustomerId,
                Email = Job.Customer.Email,
                FirstName = Job.Customer!.UserName!,
                LastName = Job.Customer.UserName!
            },
            RedirectionUrls = new RedirectionUrlsModel()
            {
                OnFailure = "https://www.facebook.com",
                OnPending = "https://www.w3schools.com/cs/cs_math.php",
                OnSuccess = "https://learn.microsoft.com/ar-sa/aspnet/core/?view=aspnetcore-8.0&utm_source=aspnet-start-page&utm_campaign=vside"
            },
            DueDate = DateTime.UtcNow.AddDays(7),
            SendEmail = true,
            Status = OrderStatus.PendingPayment,
            PayLoad = new InvoicePayload
            {
                OrderId = 1,
                OrderType = OrderType.Job,
                Provider = new ProviderModel
                {
                    Id = offer.ProviderProfileId,
                    Username = offer.ProviderProfile.User.UserName?? "UserName",
                    Email = offer.ProviderProfile.User.Email ?? "Email"
                }
            }
        };

        


        var result =await fawaterak.CreateEInvoiceAsync(eInvoiceRequestModel);


        return Success(StatusCodes.Status200OK,SuccessMessages.General.Title, SuccessMessages.General.Message);
        
    }

    public async Task<resultBase> AddJobASync(AddJobRequest request, CancellationToken cancellationToken)
    {
        var job = request.Adapt<JobPost>();
        db.JobPosts.Add(job);
        return Success(StatusCodes.Status200OK,SuccessMessages.Create.Title, SuccessMessages.Create.Message);
    }

    public Task<resultBase> AddOfferAsync(int JobId, AddJopOfferRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> CancelJobOrder(int orderId, OrderStatus orderStatus, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> ChangeSelectionOfferJob(int jobId, int oldOfferId, int newOfferId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> CompleteJobOrder(int orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> PayJobOrder(int orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> PaymentFailureJobOrder(int orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> PaymentSuccessJobOrder(int orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> RejectOfferJob(int jobId, int offerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> RevisionJobOrder(int orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> ShowOffersJob(int JobId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> StartJobOrder(int jobId, int offerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<resultBase> ViewOfferDetails(int jobId, int offerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
