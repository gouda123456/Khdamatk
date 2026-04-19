using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobOrderController(IJobOrderService jobOrderService) : ControllerBase
{
    private readonly IJobOrderService jobOrderService = jobOrderService;

    [HttpPost("Jobs/")]
    public async Task<IActionResult> AddJob(AddJobRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.AddJobASync(request, cancellationToken)).Respond();
    }
       

    [HttpPost("Jobs/{jobId}/Offers/")]
    public async Task<IActionResult> GetJob([FromRoute] int jobId, AddJopOfferRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.AddOfferAsync(jobId, request, cancellationToken)).Respond();
    }
       

    [HttpGet("Jobs/{jobId}/Offer/")]
    public async Task<IActionResult> GetJobOffers([FromRoute] int jobId, [FromQuery] string userId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.CancelJobOrder(jobId, userId, cancellationToken)).Respond();
    }
       

    

}

