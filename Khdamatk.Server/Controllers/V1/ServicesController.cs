using Khdamatk.Server.Contracts.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet("by-Category/{CategoryName}")]
    public async Task<IActionResult> GetServices([FromRoute] string CategoryName, CancellationToken ct)
    {
        var result = await _serviceService.GetCategoriesServicesAsync(CategoryName, ct);
        return result.Respond();
    }

    

    /// <summary>
    /// Get all services with optional filtering
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetServices([FromQuery] ServiceFilterRequest request, CancellationToken ct)
    {
        var result = await _serviceService.GetServicesAsync(request, ct);
        return result.Respond();
    }

    /// <summary>
    /// Get service details by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetService(int id, CancellationToken ct)
    {
        var result = await _serviceService.GetServiceAsync(id, ct);
        return result.Respond();
    }

    [HttpGet("by-service-name/{serviceName}")]
    public async Task<IActionResult> GetService(string serviceName, CancellationToken ct)
    {
        var result = await _serviceService.GetServiceAsync(serviceName, ct);
        return result.Respond();
    }

    /// <summary>
    /// Add a new service (requires authentication)
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddService([FromForm] AddServiceRequest request, CancellationToken ct)
    {
        var result = await _serviceService.AddServiceAsync(request, ct);
        return result.Respond();
    }

    /// <summary>
    /// Update an existing service (requires authentication)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateService(int id, [FromForm] UpdateServiceRequest request, CancellationToken ct)
    {
        var result = await _serviceService.UpdateServiceAsync(id, request, ct);
        return result.Respond();
    }

    /// <summary>
    /// Delete a service (requires authentication)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteService(int id, CancellationToken ct)
    {
        var result = await _serviceService.DeleteServiceAsync(id, ct);
        return result.Respond();
    }
}
