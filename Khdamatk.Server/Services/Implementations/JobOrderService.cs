using Khdamatk.Server.Contracts.orders;

namespace Khdamatk.Server.Services.Implementations;

public class JobOrderService(Database db) : IJobOrderService
{
    private readonly Database db = db;

    //Add Job and offer
    public async Task<resultBase> AddJobASync(AddJobRequest request, CancellationToken cancellationToken)
    {
        /*TODOs:
         * Validate request -Done
         * Deal with files ***
         * Mapping Request -Done
         * Add to DB -Done
         * Save changes -Done
         * send email to some of freelancers (take 20 freelancer random) {use hang fire} ***
         */

        var job = request.Adapt<JobPost>();
        await db.JobPosts.AddAsync(job, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        throw new NotImplementedException();
    }

    public Task<resultBase> AddOfferAsync(int JobId, AddJopOfferRequest request, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check job Id
         * Validate request
         * Deal with Files ***
         * Mapping request
         * add offer ti job and check the free lancer id so free lancer cant add 2 offers
         * send email to customer
         */

        throw new NotImplementedException();
    }

    public Task<resultBase> ShowOffersJob(int JobId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check job Id
         * Get Jobs 
         * convert Attachment from media to Byte[]
         * Create Offers Job Summary 
         */

        throw new NotImplementedException();
    }

    public Task<resultBase> ViewOfferDetails(int jobId, int offerId, CancellationToken cancellationToken)
    {
        /*TODOs:
         * check job id
         * check offer id
         * check if offer connect to job
         * get the Offer Details Response
         */

        throw new NotImplementedException();
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

    
}
