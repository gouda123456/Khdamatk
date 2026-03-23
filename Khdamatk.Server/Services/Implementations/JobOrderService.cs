using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.orders;
using Khdamatk.Server.Helper.Payment;

namespace Khdamatk.Server.Services.Implementations;

public class JobOrderService(Database db, IFawaterakPaymentHelper fawaterak,IWebHostEnvironment env) : IJobOrderService
{
    private readonly Database db = db;
    private readonly IFawaterakPaymentHelper fawaterak = fawaterak;
    private readonly IWebHostEnvironment env = env;


    //Add Job and offer
    public async Task<resultBase> AddJobASync(AddJobRequest request, CancellationToken cancellationToken)
    {
        /*TODOs:
         * Validate request      --Done
         * Deal with files       --***
         * Mapping Request       --Done
         * Add to DB         --Done
         * Save changes      --Done
         * send email to some of freelancers (take 20 freelancer random) {use hang fire}         --***
         */


        var job = request.Adapt<JobPost>();
        //TODO: Deal with files
        await db.JobPosts.AddAsync(job, cancellationToken);

        //TODO: send email to random 20 customer

        await db.SaveChangesAsync(cancellationToken);

        return Success(StatusCodes.Status201Created);
    }

    public async Task<resultBase> AddOfferAsync(int JobId, AddJopOfferRequest request, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check job Id Done    --Done
         * Validate request     --Done
         * Deal with Files       --***
         * add offer ti job and check the free lancer id so free lancer cant add 2 offers   --Done
         * Mapping request      --Done
         * send email to customer       --****
         */

        if (await db.JobPosts.FindAsync(JobId) is not { } Job)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if(await db.JobOffers.AnyAsync(o => o.ProviderProfileId == request.ProviderServiceId))
            return Failure(StatusCodes.Status409Conflict,FailureMessages.Conflict.Title, FailureMessages.Conflict.Message,new Error("duplicated proposal","free lancer can provide only one proposal for each job"));

        //TODO: IformFile Attachment => media 

        var offer = request.Adapt<JobOffer>();
        

        Job.Offers!.Add(offer);
        await db.SaveChangesAsync(cancellationToken);

        //TODO: Send Email to Customer

        return Success(StatusCodes.Status201Created);
    }

    public async Task<resultBase> ShowOffersJob(int JobId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check job Id     --Done
         * Get Offers       --Done
         * convert Attachment from media to Byte[]
         * Create Offers Job Summary  
         */

        if(await db.JobPosts.FirstOrDefaultAsync(j => j.Id == JobId) is not { } Job)
            return Failure(StatusCodes.Status404NotFound,FailureMessages.NotFound.Title, FailureMessages.NotFound.Message);

        var providerPic = await File.ReadAllBytesAsync(Path.Combine(env.WebRootPath, "Uploads", "Avatar.png"), cancellationToken);

        if(Job.Offers.Count() > 0) //TODO: Send Email to freeLancers
            return Failure(StatusCodes.Status404NotFound,FailureMessages.DataNotFound.Title,FailureMessages.DataNotFound.Message,new Error("there are no offers yet","there are no freelancer submit offer yet, please wait"));


        var OfferSummary = Job.Offers.Select(o => new OneOfferSummaryResponse(
            new OfferServiceDetailed(
                o.Id,
                o.Amount,
        DateTime.UtcNow.AddDays(o.DeliveryTimeInDays),
        o.Description
                ),
            new  ProviderOfferInfo(o.ProviderProfileId,
            o.ProviderProfile.User.UserName!,
            o.ProviderProfile.JobTitle,
            o.ProviderProfile.AverageRating,
            providerPic)));

        

        return Success(StatusCodes.Status200OK,SuccessMessages.General.Title, SuccessMessages.General.Message,OfferSummary);
    }


    //Done
    public async Task<resultBase> ViewOfferDetails(int jobId, int offerId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check job id     --Done
         * check offer id       --Done
         * check if offer connect to job        --Done
         * get the Offer Details Response       --Done
         */

        if(!db.JobPosts.Any(j => j.Id == jobId && j.Offers.Any(o => o.Id == offerId)))
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        var offerDetail = db.JobOffers.Find(offerId).Adapt<OfferDetailedForServiceResponse>();

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, offerDetail);

    }
    

    //initialize order by select, change offer and start payment

    public Task<resultBase> StartJobOrder(int jobId, int offerId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check if job downt have any order
         * start order
         * order have 
         * {
             * job and offer id,
             * expected time from offer.DeliveryTimeInDays,
             * customer and freelancer IDs,
             * create conversation,
             * job Deliverables,
             * Media.Attachments 
         * }
         * Start payment 
         * send email to customer 
         * send email to freelancer
         * return statues
         */



        throw new NotImplementedException();
    }

    public Task<resultBase> ChangeSelectionOfferJob(int OrderId, int oldOfferId, int newOfferId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * validate old offer == order.AcceptedOfferId
         * check if new offer id is exist
         * in order change offer id from old to new
         * Update order have 
         * {
             * job and offer id,
             * expected time from offer.DeliveryTimeInDays,
             * customer and freelancer IDs,
             * create conversation,
             * job Deliverables,
             * Media.Attachments,
             * statues = payment (new offer net - old)
             * (enhancement feature: add Amount in User Entity + add endpoint to pay for user Amount + refunds + checkout (pull money) )
         * }
         * Start payment 
         * send email to customer 
         * send email to freelancer
         * send email to freelancer who old offer
         * send email to freelancer who new offer
         * return Statues
         */
        throw new NotImplementedException();
    }

    public Task<resultBase> RejectOfferJob(int OrderId, int offerId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check if order.AcceptedOfferId == offerId
         * change order.AcceptedOfferId
         * send Payment
         */
        //Send email
        throw new NotImplementedException();
    }
    
    
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

    //order statues: Cancel , Failed , Success 

    public Task<resultBase> CancelJobOrder(int orderId, string userId, OrderStatus orderStatus, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check user id to determine who cancel the order (customer , freelancer)
         * if Customer and order.state == in progress =>
         * {
             * customer.Amount += offer.Amount ,
             * order.state = cancelByCustomer ,
             * send email to freeLancer
         * }
         * if Freelancer and order.state == in progress => order.state = cancelByFreeLancer + send email to Customer
         * 
         */
        throw new NotImplementedException();
    }

    public Task<resultBase> PaymentFailureJobOrder(int orderId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * send email to customer (Payment fail)
         * (Feature enhancement: Add fail payment transaction list to store failed transactions)
         */
        throw new NotImplementedException();
    }

    public Task<resultBase> PaymentSuccessJobOrder(int orderId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * order.state = in progress
         * add transactions
         * send email to Customer
         * send email to Free Lancer
         */
        throw new NotImplementedException();
    }


    //order Detail 

    public Task<resultBase> OrderSummary(int orderId, string userId)
    {
        /*TODOs:
         * check if order is exists 
         * mapping job order to Contract.jobOrderSummary 
         * 
         */
        throw new NotImplementedException();
    }

    public Task<resultBase> OrderDetails(int orderId, string userId)
    {
        /*TODOs:
         * check if order is exists 
         * mapping job order to Contract.jobOrderDetailed 
         * 
         */
        throw new NotImplementedException();
    }

    public Task<resultBase> SubmitWorkAndMessage(int orderId, SubmitWorkAndMessageRequest request)
    {
        /*TODOs:
         * check if order is exists and state == in progress
         * Validate request  
         * check if there any attachments 
         * add to conversation
         * convert List<IFormFile> to Media  
         * (Feature:IFormFile.ToMedia(),list<IFormFile>): (params IFormFile[] Medias) => {store Data in project + convert IFormFile to media entity}
         */
        throw new NotImplementedException();
    }

    public Task<resultBase> CompleteJobOrder(int orderId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check if order.state == in progress
         * add offer Amount to free lancer
         * order.state = complete
         * send email to free lancer
         */
        throw new NotImplementedException();
    }

    public Task<resultBase> OpenDispute(int orderId, string RaiserId, string ReasonDetails, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check if order.state == in progress
         * compare the RaiserId to know if it customer or freelancer
         * send email to the 3 party (customer , freelancer , admins)
         * create Dispute object
         */
        throw new NotImplementedException();
    }

    public Task<resultBase> RevisionJobOrder(int orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
