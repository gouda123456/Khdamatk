using Khdamatk.Server.Contracts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]

public class ServiceOrderController(IServiceOrderService serviceOrderService) : ControllerBase
{
    private readonly IServiceOrderService serviceOrderService = serviceOrderService;

    [AllowAnonymous]
    [HttpPost("add-order")]
    public async Task<IActionResult> AddOrder([FromBody] CreateJobOrderRequest request, CancellationToken cancellationToken)
    {
        // 1. لازم تجيب الـ UserId بتاع الشخص اللي عامل Login حالياً (العميل)
        var userId = "1";

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // 2. ابعت الـ userId للـ Service لأنها مستنياه في الترتيب التاني
        var result = await serviceOrderService.AddOrderAsync1(request, userId, cancellationToken);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : BadRequest(result);
    }

    // 2. قبول الأوردر (من طرف الفريلانسر)
    [HttpPut("accept-order/{orderId}")]
    public async Task<IActionResult> AcceptOrder(int orderId, CancellationToken cancellationToken)
    {
        // هنا بنجيب الـ UserId بتاع الشخص اللي عامل Login حالياً (الفريلانسر)
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await serviceOrderService.AcceptOrderAsync(orderId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // 3. رفض الأوردر (من طرف الفريلانسر)
    [HttpPut("reject-order/{orderId}")]
    public async Task<IActionResult> RejectOrder(int orderId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await serviceOrderService.RejectOrderAsync(orderId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);

    }

    // 1. تجيب كل أوردرات المستخدم (My Orders)
    [HttpGet("my-orders")]
    public async Task<IActionResult> GetOrders()
    {
        // بنجيب الـ UserId من الـ Claims بتاعة الـ Token
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await serviceOrderService.GetUserOrders(userId);
        return result.Respond(); // تأكد إن عندك Extension Method اسمها Respond بتتعامل مع resultBase
    }

    // 2. تجيب أوردر واحد محدد بالـ ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await serviceOrderService.GetOrderById(id, userId);
        return result.Respond();
    }

    /// //////////s////////

    [HttpGet("GetServices")]
    public async Task<resultBase> GetServices([FromQuery] GetServicesRequest request, CancellationToken ct)
    {
        // لو الـ request جاي فاضي، الـ Service هترجع كل الداتا تلقائياً
        return await serviceOrderService.GetServices(request, ct);
    }

    // 2. الحصول على خدمة واحدة بالتفصيل
    [HttpGet("GetService/{id}")]
    public async Task<resultBase> GetService(int id, CancellationToken ct)
    {
        return await serviceOrderService.GetServiceById(id, ct);
    }

    // 3. إضافة خدمة جديدة
    [HttpPost("AddService")]
    public async Task<resultBase> AddService([FromBody] AddServiceRequest1 request, CancellationToken ct)
    {
        return await serviceOrderService.AddService(request, ct);
    }

    // 4. تحديث خدمة موجودة
    [HttpPut("UpdateService/{id}")]
    public async Task<resultBase> UpdateService(int id, [FromBody] UpdateServiceRequest request, CancellationToken ct)
    {
        return await serviceOrderService.UpdateService(id, request, ct);
    }

    // 5. حذف خدمة (Soft Delete)
    [HttpDelete("DeleteService/{id}")]
    public async Task<resultBase> DeleteService(int id, CancellationToken ct)
    {
        return await serviceOrderService.DeleteService(id, ct);
    }
}


