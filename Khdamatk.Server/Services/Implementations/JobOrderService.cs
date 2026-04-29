using Khdamatk.Server.Contracts.Conversations;
using Khdamatk.Server.Contracts.WebHook;
using Khdamatk.Server.Helper.Payment;
using Stripe.Climate;
using System.Text.Json;

namespace Khdamatk.Server.Services.Implementations;

public class JobOrderService(Database db, IFawaterakPaymentHelper fawaterak,IWebHostEnvironment env,IOptions<ClientSetting> options) : IJobOrderService
{
    private readonly Database db = db;
    private readonly IFawaterakPaymentHelper fawaterak = fawaterak;
    private readonly IWebHostEnvironment env = env;
    private readonly ClientSetting clientSetting = options.Value;


    //Add Job and offer
    public async Task<resultBase> AddJobASync(AddJobRequest request, CancellationToken cancellationToken = default)
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

    public async Task<resultBase> AddOfferAsync(int JobId, AddJopOfferRequest request, CancellationToken cancellationToken = default)
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

    public async Task<resultBase> ShowOffersJob(int JobId, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check job Id     --Done
         * Get Offers       --Done
         * convert Attachment from media to Byte[]
         * Create Offers Job Summary  
         */

        if(await db.JobPosts.FirstOrDefaultAsync(j => j.Id == JobId) is not { } Job)
            return Failure(StatusCodes.Status404NotFound,FailureMessages.NotFound.Title, FailureMessages.NotFound.Message);

        var providerPic = await System.IO.File.ReadAllBytesAsync(Path.Combine(env.WebRootPath, "Uploads", "Avatar.png"), cancellationToken);

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

    public async Task<resultBase> ViewOfferDetails(int jobId, int offerId, CancellationToken cancellationToken = default)
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

    public async Task<resultBase> StartJobOrder(int jobId, int offerId, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check if job do not have any order       --Done
         * start order      --Done
         * order have       --Done
         * {
             * job and offer id,
             * expected time from offer.DeliveryTimeInDays,
             * customer and freelancer IDs,
             * create conversation,
             * job Deliverables,
             * Media.Attachments            --Wtf???     
         * }
         * Start payment            --Done
         * send email to customer 
         * send email to freelancer
         * return statues
         */

        if(await CheckJobAndOfferAsync(jobId, offerId,cancellationToken))
            return Failure(StatusCodes.Status404NotFound,FailureMessages.NotFound.Title, FailureMessages.NotFound.Message,
                new Error("job does not exist","there are no job linked to this Id"));

        JobPost? job = await db.JobPosts.FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken: cancellationToken);
        JobOffer? offer = await db.JobOffers.FirstOrDefaultAsync(o => o.Id == jobId && o.JobPostId == jobId, cancellationToken:cancellationToken);

        if(job!.OrderId != null)
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, FailureMessages.Conflict.Message,
                new Error("job is already linked to order", "there are order linked to this job please change offer or check on order details "));

        

        var order = JobOrder.BuildOrder(job, offer!);


        await db.JobOrders.AddAsync(order, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        order.Conversation.RelatedEntityId = order.Id;
        order.Conversation.JobOrderId = order.Id;
        await db.SaveChangesAsync(cancellationToken);


        EInvoiceRequestModel eInvoice = new()
        {
            Currency = CurrencyCode.EGP.ToString(),
            DueDate = order.ExpectedDeliveryDate,
            SendEmail = true,
            Status = order.Status,
            RedirectionUrls = new RedirectionUrlsModel()
            {
                OnSuccess = clientSetting + "/DashBoard?State=OnSuccess",
                OnFailure = clientSetting + "/DashBoard?State=OnFailure",
                OnPending = clientSetting + "/DashBoard?State=OnPending",
            },
            PayLoad = new InvoicePayload()
            {
                OrderId = order.Id,
                OrderType = OrderType.Job,
                Provider = new ProviderModel()
                {
                    Id = order.ServiceProviderId,
                    Email = order.ServiceProviderProfile.User.Email!,
                    Username = order.ServiceProviderProfile.User.UserName!
                }
            },
            Customer = new CustomerModel()
            {
                CustomerId = order.CustomerId,
                FirstName = order.Customer.UserName!,
                LastName = order.Customer.UserName!,
                Email = order.Customer.Email,

            },
            CartItems = new()
            {
                new CartItemModel()
                {
                    Name = order.Job.Title,
                    Price = order.AcceptedOffer.Amount,
                    Quantity = 1
                }
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

    public async Task<resultBase> AcceptOfferJob(int jobId, int offerId, string CustomerId, CancellationToken cancellationToken = default)
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


        var order = JobOrder.BuildOrder(Job, offer);


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
                    Price=offer.Amount,
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
                    Username = offer.ProviderProfile.User.UserName ?? "UserName",
                    Email = offer.ProviderProfile.User.Email ?? "Email"
                }
            }
        };




        var result = await fawaterak.CreateEInvoiceAsync(eInvoiceRequestModel);

        if(result != null)
        {
            order.InvoiceId = result.InvoiceId;
            order.InvoiceKey = result.InvoiceKey;
            await db.SaveChangesAsync(cancellationToken);
        }
        
         
        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);

    }

    public async Task<resultBase> ChangeSelectionOfferJob(int OrderId, int oldOfferId, int newOfferId, string userId, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * validate old offer == order.AcceptedOfferId          --Done
         * check if new offer id is exist               --Done
         * in order change offer id from old to new         --Done
         * Update order have 
         * {
             * job and offer id,            --Done
             * expected time from offer.DeliveryTimeInDays,         --Done
             * customer and freelancer IDs,             --Ignored(no need for customer)
             * create conversation,             --Done
             * job Deliverables,
             * Media.Attachments,           --Ignored
             * statues = payment (new offer net - old)          --No need offer can change only before payment
             * (enhancement feature: add Amount in User Entity + add endpoint to pay for user Amount + refunds + checkout (pull money) )    --Not Done yet
         * }
         * Start payment            --Done
         * send email to customer 
         * send email to freelancer who old offer
         * send email to freelancer who new offer
         * return Statues
         */

        var order = await db.JobOrders.FirstOrDefaultAsync(o => o.Id == OrderId && o.CustomerId == userId);

        if (order == null)
        {
            return Failure(StatusCodes.Status404NotFound, new Error("order not Found", "there are no order with this id"));
        }

        if (order.AcceptedOfferId != oldOfferId && order.AcceptedOfferId == newOfferId && order.Status == OrderStatus.PendingPayment)
        {
            return Failure(StatusCodes.Status409Conflict, new Error("Conflict in offer Ids", "the old offer dosent match the new or it match the selected"));
        }

        var newOffer = await db.JobOffers.FirstOrDefaultAsync(o => o.Id == newOfferId && o.JobPostId == order.JobPostId);

        if (newOffer == null)
        {
            return Failure(StatusCodes.Status404NotFound, new Error("new offer not Found", "there are no offer with this id"));
        }

        order.AcceptedOfferId = newOfferId;
        order.Amount = newOffer.Amount;
        order.ExpectedDeliveryDate = DateTime.UtcNow.AddDays(newOffer.DeliveryTimeInDays);
        order.ServiceProviderId = newOffer.ProviderProfileId;
        order.Conversation.ProviderId = order.AcceptedOffer.ProviderProfileId;



        await db.SaveChangesAsync(cancellationToken);


        EInvoiceRequestModel eInvoice = new EInvoiceRequestModel()
        {
            Currency = CurrencyCode.EGP.ToString(),
            DueDate = order.ExpectedDeliveryDate,
            SendEmail = true,
            Status = order.Status,
            RedirectionUrls = new RedirectionUrlsModel()
            {
                OnSuccess = clientSetting + "/DashBoard?State=OnSuccess",
                OnFailure = clientSetting + "/DashBoard?State=OnFailure",
                OnPending = clientSetting + "/DashBoard?State=OnPending",
            },
            PayLoad = new InvoicePayload()
            {
                OrderId = order.Id,
                OrderType = OrderType.Job,
                Provider = new ProviderModel()
                {
                    Id = order.ServiceProviderId,
                    Email = order.ServiceProviderProfile.User.Email!,
                    Username = order.ServiceProviderProfile.User.UserName!
                }
            },
            Customer = new CustomerModel()
            {
                CustomerId = order.CustomerId,
                FirstName = order.Customer.UserName!,
                LastName = order.Customer.UserName!,
                Email = order.Customer.Email,

            },
            CartItems = new()
            {
                new CartItemModel()
                {
                    Name = order.Job.Title,
                    Price = order.AcceptedOffer.Amount,
                    Quantity = 1
                }
            }
        };


        var result = await fawaterak.CreateEInvoiceAsync(eInvoice);
        if (result != null)
        {
            order.InvoiceId = result.InvoiceId;
            order.InvoiceKey = result.InvoiceKey;
            await db.SaveChangesAsync(cancellationToken);

            //TODO: send email to customer 
            //TODO: send email to freelancer who has old offer
            //TODO: send email to freelancer who has new offer

            return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
        }

        //TODO: send email to customer (Failed)


        return Failure(StatusCodes.Status503ServiceUnavailable, new Error("Payment Gateway Error","The Payment Service Not Available"));
    }

    public async Task<resultBase> RejectOfferJob(int jobId, int offerId, CancellationToken cancellationToken = default)
    {

        /*TODOs:
         * check if job.offer have offerId        --Done (with diffrent way)
         * Delete offer                             --Done
         * send email to Provider who have offer 
         */

        
        var offer = await db.JobOffers.FirstOrDefaultAsync(o => o.Id == offerId && o.JobPostId == jobId, cancellationToken: cancellationToken);
        if (offer == null)
            return Failure(StatusCodes.Status404NotFound, new Error("Job or offer not Found", "there are no Job or offer with this id"));

        offer.Status = JobOfferStatus.Rejected;
        await db.SaveChangesAsync(cancellationToken);

        // send email to Provider who have offer

        return Success(StatusCodes.Status204NoContent);
    }


    //order statues: Cancel , Failed , Success 

    //TODO: add cancel reason enum (cancel by customer , cancel by freelancer , cancel by system (payment failure or dispute) )
    //TODO: Add Refund process (Feature enhancement: Add Amount in User Entity + add endpoint to pay for user Amount + refunds + checkout (pull money) )    --Not Done yet
    public async Task<resultBase> CancelJobOrder(int orderId, string userId, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check user id to determine who cancel the order (customer , freelancer)      --Done
         * if Customer and order.state == in progress =>        --Half Done (order state)
         * {
             * customer.Amount += offer.Amount ,
             * order.state = cancelByCustomer ,
             * send email to freeLancer
         * }
         * if Freelancer and order.state == in progress => order.state = cancelByFreeLancer + send email to Customer
         * 
         */

        var order = await db.JobOrders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken: cancellationToken);
        if(order == null)
            return Failure(StatusCodes.Status404NotFound,FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);


        //TODO: Add Amount in User Entity + add endpoint to pay for user Amount + refunds + checkout (pull money) )    --Not Done yet
        if (order.CustomerId == userId)
        {
            order.Status = OrderStatus.CancelledByClient;
            
            //TODO: send email to Customer
        }




        if (order.ServiceProviderId == userId)
        {
            order.Status = OrderStatus.CancelledByProvider;
            //TODO: send email to freeLancer
        }

        if(order.Status == OrderStatus.Active)
            order.Customer.Amount += order.AcceptedOffer.Amount;



        

        await db.SaveChangesAsync(cancellationToken);
        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);

        
    }

    public async Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * send email to customer (Payment fail)
         * (Feature enhancement: Add fail payment transaction list to store failed transactions)
         */

        //TODO: send email to customer (Payment fail)

        

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
    }

    public async Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * order.state = in progress        --Done
         * add transactions                 --Half Done (add transaction but not with all details)
         * send email to Customer
         * send email to Free Lancer
         */

        model.Payload = model.PayloadString != null ? JsonSerializer.Deserialize<InvoicePayload>(model.PayloadString) : null;

        if (model.Payload != null)
        {
            return Failure(StatusCodes.Status400BadRequest, new Error("Invalid Payload", "The payload data is invalid or missing"));
        }

        var order = await db.JobOrders.FirstOrDefaultAsync(o => o.Id == model.Payload!.OrderId && o.InvoiceKey == model.InvoiceKey, cancellationToken: cancellationToken);

        if(order == null)
        {
            return Failure(StatusCodes.Status404NotFound, new Error("Order Not Found", "There are no order matching the provided details"));
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


    //order Detail 

    public async Task<resultBase> OrderSummary(int orderId, string userId)
    {
        /*TODOs:
         * check if order is exists 
         * mapping job order to Contract.jobOrderSummary 
         */

        var file = System.IO.File.ReadAllBytes(Path.Combine(env.WebRootPath, "Uploads", "Avatar.png"));

        var orderSummary = await db.JobOrders.Where(o => o.Id == orderId && (o.CustomerId == userId || o.ServiceProviderId == userId))
            .ProjectToType<JobOrderResponse>().FirstOrDefaultAsync();

        if (orderSummary == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);



        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, orderSummary);
    }

    public async Task<resultBase> OrderDetails(int orderId, string userId)
    {
        /*TODOs:
         * check if order is exists         --Done
         * mapping job order to Contract.jobOrderDetailed       --Done (Need to Test it (Attachment and Conversation))
         * 
         */


        var file = System.IO.File.ReadAllBytes(Path.Combine(env.WebRootPath, "Uploads", "Avatar.png"));

        

        var orderDetail = await db.JobOrders.Where(o => o.Id == orderId && (o.CustomerId == userId || o.ServiceProviderId == userId))
            .ProjectToType<JobOrderResponse>().FirstOrDefaultAsync();

        if (orderDetail == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        


        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, orderDetail);
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

        var Joborder = await db.JobOrders.FirstOrDefaultAsync(o => o.Id == orderId && (o.CustomerId == userId || o.ServiceProviderId == userId));
        

        if (Joborder == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if(request.Attachments != null && request.Attachments.Count > 0)
        {
            //TODO: convert List<IFormFile> to List<Media> then save it in DB with relation to order
            //TODO: store files in project (wwwroot/Uploads/JobOrderId/)

        }

        Joborder.Conversation.Messages.Add(new ()
        {
            SenderId = userId,
            Content = request.Message,
            IsRead = false,
        });

        await db.SaveChangesAsync(cancellationToken);

        return await OrderDetails(orderId, userId);


    }

    public async Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default)
    {
        var conversations = await db.JobOrders.Where(c => c.CustomerId == userId || c.ServiceProviderId == userId).
            Select(c => new ConversationsSummaryResponse(
                userId,
        (userId == c.CustomerId) ? c.Customer.UserName : c.ServiceProviderProfile.User.UserName,
        (userId == c.CustomerId) ? c.Customer.ProfilePicture.FullPath : c.ServiceProviderProfile.User.ProfilePicture.FullPath,
        c.Job.Title,
        c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault()!.Content : "",
                c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault()!.Createdat : DateTime.MinValue,
                c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault()!.IsRead : true
                 )
                ).ToListAsync(cancellationToken: cancellationToken);
            

        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, conversations);
    }
    public async Task<resultBase> GetConversationMessages(int orderId, string UserId, CancellationToken cancellationToken = default)
    {
        var order = await db.JobOrders.FirstOrDefaultAsync(o => o.Id == orderId && (o.CustomerId == UserId || o.ServiceProviderId == UserId), cancellationToken: cancellationToken);

        var converationDetailed = new ConversationsDetailed(
            order.Conversation.Id,
            order.Job.Title,
            UserId,
            (UserId == order.CustomerId) ? order.Customer.UserName : order.ServiceProviderProfile.User.UserName,
            (UserId == order.CustomerId) ? order.Customer.ProfilePicture.FullPath : order.ServiceProviderProfile.User.ProfilePicture.FullPath,
            (UserId != order.CustomerId)? order.CustomerId : order.ServiceProviderId,
            (UserId != order.CustomerId)? order.Customer.UserName : order.ServiceProviderProfile.User.UserName, 
            (UserId != order.CustomerId)? order.Customer.ProfilePicture.FullPath : order.ServiceProviderProfile.User.ProfilePicture.FullPath,
            order.Conversation.Messages.Select(m => new ConversationMessageResponse(m.Id, m.Content, m.SenderId, m.Createdat)).ToList()
            );


        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message,converationDetailed);
    }

    public async Task<resultBase> CompleteJobOrder(int orderId, ReviewRequest request, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check if order.state == in progress      --Done
         * add offer Amount to free lancer          --Done (without refund process)
         * add review to order            --Done
         * order.state = complete           --Done
         * send email to free lancer
         */

        var order = await db.JobOrders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order == null)
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title, FailureMessages.DataNotFound.Message);

        if (order.Status != OrderStatus.Active)
            return Failure(StatusCodes.Status409Conflict, FailureMessages.Conflict.Title, FailureMessages.Conflict.Message);

        var freelancer = await db.ServiceProviderProfiles.FirstOrDefaultAsync(s => s.UserId == order.ServiceProviderId, cancellationToken);

        //TODO: Add endpoint to pay for user Amount + refunds + checkout (pull money) )    --Not Done yet
        
        freelancer!.User.Amount += order.AcceptedOffer.Amount; 

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

    public async Task<resultBase> OpenDispute(int orderId, string RaiserId, string ReasonDetails, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * check if order.state == in progress          --Done
         * compare the RaiserId to know if it customer or freelancer            --Done
         * create Dispute object            --Done
         * send email to the 3 party (customer , freelancer , admins)               ***
         */

        var order = await db.JobOrders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken = default);

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
            order.Dispute = new ()
            {
                JobOrderId = orderId,
                RaiserId = RaiserId,
                ReasonDetails = ReasonDetails
            };
        }



        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);

    }


    // 1. إضافة طلب جديد (بناءً على شغلانة وعرض سعر مقبول)
    public async Task<resultBase> AddOrderAsync(CreateJobOrderRequest request, string customerId, CancellationToken cancellationToken = default)
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

    private async Task<bool> CheckJobAsync(int JobId, CancellationToken cancellationToken = default)
        => await db.JobPosts.AnyAsync(j => j.Id == JobId, cancellationToken: cancellationToken);

    private async Task<bool>  CheckJobAndOfferAsync(int JobId, int OfferId, CancellationToken cancellationToken = default)
        => await db.JobPosts.AnyAsync(j => j.Id == JobId && j.Offers.Any(o => o.Id == OfferId), cancellationToken: cancellationToken);
    
}