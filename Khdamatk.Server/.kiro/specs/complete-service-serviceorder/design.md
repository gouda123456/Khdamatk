# Design: Complete Service & ServiceOrder Implementation

## Architecture Overview

هذا التصميم يتبع نفس البنية المعمارية المستخدمة في Job/JobOrder system:

- **Layered Architecture**: Controllers → Services → Data Access
- **Repository Pattern**: للوصول إلى قاعدة البيانات
- **DTO Pattern**: استخدام Contracts للـ Request/Response models
- **Dependency Injection**: لجميع الـ Services

## Component Design

### 1. Contracts Layer

#### 1.1 Service Contracts (`Contracts/Service/`)

**Existing Files** (سيتم الاحتفاظ بها):

- `AddServiceRequest.cs` ✅
- `ServiceDetailsResponse.cs` ✅
- `OrderServiceRequest.cs` ✅

**New Files** (سيتم إنشاؤها):

##### `ServiceSummaryResponse.cs`

```csharp
public record ServiceSummaryResponse(
    int Id,
    string Title,
    string ShortDescription,
    decimal Price,
    byte[] MainImage,
    int OrdersCount,
    double AverageRating,
    int DeliveryTimeInDays,
    ProviderSummaryInfo ProviderInfo
);

public record ProviderSummaryInfo(
    string Id,
    string Name,
    byte[] ProfileImage,
    double Rating
);
```

##### `UpdateServiceRequest.cs`

```csharp
public record UpdateServiceRequest(
    string Title,
    string CategoryName,
    string ShortDescription,
    string DetailedDescription,
    decimal Price,
    int RevisionCount,
    List<string> Concepts,
    int DeliverTimeInDays,
    ExperienceLevel ExperienceLevel,
    Media? ServiceEnvelope,
    List<IFormFile>? Attachment
);

public class UpdateServiceRequestValidator : AbstractValidator<UpdateServiceRequest>
{
    // Similar to AddServiceValidator
}
```

##### `ServiceFilterRequest.cs`

```csharp
public record ServiceFilterRequest(
    string? SearchTerm,
    string? CategoryName,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? MinDeliveryDays,
    int? MaxDeliveryDays,
    ExperienceLevel? ExperienceLevel,
    double? MinRating,
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = "CreatedAt",
    bool SortDescending = true
);
```

#### 1.2 ServiceOrder Contracts (`Contracts/orders/`)

**New Files**:

##### `ServiceOrderResponse.cs`

```csharp
public record ServiceOrderResponse(
    int OrderId,
    OrderType OrderType,
    OrderStatus Status,
    decimal FinalPrice,
    UserOrderModel Customer,
    UserOrderModel Provider,
    ServiceSummary ServiceSummary,
    List<OrderChat> Chat,
    List<DeliverableFiles> DeliverableFiles,
    DateTime CreatedAt,
    DateTime? CompletedAt
);

public record ServiceSummary(
    int Id,
    string Title,
    decimal Price,
    int DeliveryTimeInDays,
    int RevisionCount,
    string Description
);
```

##### `ServiceOrderSummaryResponse.cs`

```csharp
public record ServiceOrderSummaryResponse(
    int OrderId,
    OrderType OrderType,
    OrderStatus Status,
    decimal FinalPrice,
    string CustomerName,
    string ProviderName,
    string ServiceTitle,
    DateTime CreatedAt,
    DateTime? Deadline,
    int UnreadMessagesCount
);
```

##### `ServiceOrderFilterRequest.cs`

```csharp
public record ServiceOrderFilterRequest(
    OrderStatus? Status,
    DateTime? FromDate,
    DateTime? ToDate,
    decimal? MinPrice,
    decimal? MaxPrice,
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = "CreatedAt",
    bool SortDescending = true
);
```

### 2. Service Layer

#### 2.1 Service Interface (`Services/Interfaces/IServiceService.cs`)

**New File**:

```csharp
public interface IServiceService : IService
{
    Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken ct = default);
    Task<resultBase> GetServiceAsync(int serviceId, CancellationToken ct = default);
    Task<resultBase> GetServicesAsync(ServiceFilterRequest request, CancellationToken ct = default);
    Task<resultBase> UpdateServiceAsync(int serviceId, UpdateServiceRequest request, CancellationToken ct = default);
    Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken ct = default);
    Task<resultBase> GetProviderServicesAsync(string providerId, CancellationToken ct = default);
}
```

#### 2.2 ServiceOrder Interface Refactoring (`Services/Interfaces/IServiceOrderService.cs`)

**Refactored File**:

```csharp
public interface IServiceOrderService : IService
{
    // Order Creation & Initialization
    Task<resultBase> AddOrderAsync(int serviceId, string customerId, OrderServiceRequest request, CancellationToken ct = default);
    Task<resultBase> AcceptOrderAsync(int orderId, string providerId, CancellationToken ct = default);
    Task<resultBase> RejectOrderAsync(int orderId, string providerId, CancellationToken ct = default);

    // Payment
    Task<resultBase> StartServiceOrderPaymentAsync(int orderId, CancellationToken ct = default);
    Task<resultBase> PaymentSuccessAsync(WebHookModel model, CancellationToken ct = default);
    Task<resultBase> PaymentFailureAsync(CancelTransactionModel model, CancellationToken ct = default);

    // Order Management
    Task<resultBase> GetOrderAsync(int orderId, string userId, CancellationToken ct = default);
    Task<resultBase> GetOrdersAsync(string userId, ServiceOrderFilterRequest request, CancellationToken ct = default);
    Task<resultBase> GetOrderSummaryAsync(int orderId, string userId, CancellationToken ct = default);
    Task<resultBase> CancelOrderAsync(int orderId, string userId, CancellationToken ct = default);

    // Communication & Delivery
    Task<resultBase> SubmitWorkAndMessageAsync(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken ct = default);
    Task<resultBase> GetConversationsAsync(string userId, CancellationToken ct = default);
    Task<resultBase> GetConversationMessagesAsync(int orderId, string userId, CancellationToken ct = default);

    // Order Completion
    Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken ct = default);
    Task<resultBase> OpenDisputeAsync(int orderId, string raiserId, OrderDisputeRequest request, CancellationToken ct = default);
}
```

#### 2.3 Service Implementation (`Services/Implementations/ServiceService.cs`)

**New File Structure**:

```csharp
public class ServiceService : IServiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService,
        ILogger<ServiceService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken ct)
    {
        // 1. Validate provider exists
        // 2. Validate category exists
        // 3. Process and save images/attachments
        // 4. Create service entity
        // 5. Save to database
        // 6. Return ServiceDetailsResponse
    }

    public async Task<resultBase> GetServiceAsync(int serviceId, CancellationToken ct)
    {
        // 1. Get service with related data (provider, category, images, reviews)
        // 2. Calculate average rating
        // 3. Map to ServiceDetailsResponse
        // 4. Return result
    }

    public async Task<resultBase> GetServicesAsync(ServiceFilterRequest request, CancellationToken ct)
    {
        // 1. Build query with filters
        // 2. Apply sorting
        // 3. Apply pagination
        // 4. Map to ServiceSummaryResponse list
        // 5. Return paginated result
    }

    public async Task<resultBase> UpdateServiceAsync(int serviceId, UpdateServiceRequest request, CancellationToken ct)
    {
        // 1. Get existing service
        // 2. Verify ownership
        // 3. Update properties
        // 4. Process new images if provided
        // 5. Save changes
        // 6. Return updated service
    }

    public async Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken ct)
    {
        // 1. Get service
        // 2. Verify ownership
        // 3. Check if service has active orders
        // 4. Soft delete (set IsDeleted = true)
        // 5. Return success
    }
}
```

#### 2.4 ServiceOrder Implementation Refactoring (`Services/Implementations/ServiceOrderService.cs`)

**Refactored Structure**:

```csharp
public class ServiceOrderService : IServiceOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<ServiceOrderService> _logger;

    // Order Lifecycle Methods following JobOrderService pattern:

    // 1. Order Creation Phase
    public async Task<resultBase> AddOrderAsync(...)
    {
        // - Validate service exists and is active
        // - Validate customer
        // - Create order with status = PendingProviderAcceptance
        // - Send notification to provider
    }

    public async Task<resultBase> AcceptOrderAsync(...)
    {
        // - Verify provider ownership
        // - Update status to PendingPayment
        // - Send notification to customer
    }

    public async Task<resultBase> RejectOrderAsync(...)
    {
        // - Verify provider ownership
        // - Update status to Rejected
        // - Send notification to customer
    }

    // 2. Payment Phase
    public async Task<resultBase> StartServiceOrderPaymentAsync(...)
    {
        // - Verify order status
        // - Create payment invoice via Fawaterak
        // - Return payment URL
    }

    public async Task<resultBase> PaymentSuccessAsync(...)
    {
        // - Update order status to InProgress
        // - Record payment details
        // - Send notifications
    }

    // 3. Execution Phase
    public async Task<resultBase> SubmitWorkAndMessageAsync(...)
    {
        // - Save message
        // - Save deliverable files
        // - Update order status if needed
        // - Send notification
    }

    // 4. Completion Phase
    public async Task<resultBase> CompleteOrderAsync(...)
    {
        // - Verify customer ownership
        // - Update status to Completed
        // - Save review
        // - Update provider rating
        // - Release payment
    }

    // 5. Dispute Phase
    public async Task<resultBase> OpenDisputeAsync(...)
    {
        // - Create dispute record
        // - Update order status to Disputed
        // - Notify admin and other party
    }
}
```

### 3. Controller Layer

#### 3.1 ServicesController (`Controllers/V1/ServicesController.cs`)

**New File**:

```csharp
[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetServices([FromQuery] ServiceFilterRequest request, CancellationToken ct)
    {
        var result = await _serviceService.GetServicesAsync(request, ct);
        return result.Respond();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetService(int id, CancellationToken ct)
    {
        var result = await _serviceService.GetServiceAsync(id, ct);
        return result.Respond();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddService([FromForm] AddServiceRequest request, CancellationToken ct)
    {
        var result = await _serviceService.AddServiceAsync(request, ct);
        return result.Respond();
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateService(int id, [FromForm] UpdateServiceRequest request, CancellationToken ct)
    {
        var result = await _serviceService.UpdateServiceAsync(id, request, ct);
        return result.Respond();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteService(int id, CancellationToken ct)
    {
        var result = await _serviceService.DeleteServiceAsync(id, ct);
        return result.Respond();
    }
}
```

#### 3.2 ServiceOrderController Refactoring (`Controllers/V1/ServiceOrderController.cs`)

**Refactored Structure** (following JobOrderController pattern):

```csharp
[Route("api/[controller]")]
[ApiController]
public class ServiceOrderController : ControllerBase
{
    private readonly IServiceOrderService _serviceOrderService;

    public ServiceOrderController(IServiceOrderService serviceOrderService)
    {
        _serviceOrderService = serviceOrderService;
    }

    #region Order Initialization

    [HttpPost("Services/{serviceId}/Orders")]
    [Authorize]
    public async Task<IActionResult> CreateOrder(
        [FromRoute] int serviceId,
        [FromBody] OrderServiceRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.AddOrderAsync(serviceId, userId, request, ct)).Respond();
    }

    [HttpPut("Services/{serviceId}/Orders/{orderId}/Accept")]
    [Authorize]
    public async Task<IActionResult> AcceptOrder(
        [FromRoute] int serviceId,
        [FromRoute] int orderId,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.AcceptOrderAsync(orderId, userId, ct)).Respond();
    }

    [HttpPut("Services/{serviceId}/Orders/{orderId}/Reject")]
    [Authorize]
    public async Task<IActionResult> RejectOrder(
        [FromRoute] int serviceId,
        [FromRoute] int orderId,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.RejectOrderAsync(orderId, userId, ct)).Respond();
    }

    [HttpPost("ServiceOrders/{orderId}/Payment")]
    [Authorize]
    public async Task<IActionResult> StartPayment(
        [FromRoute] int orderId,
        CancellationToken ct)
    {
        return (await _serviceOrderService.StartServiceOrderPaymentAsync(orderId, ct)).Respond();
    }

    #endregion

    #region Order Middle Operations

    [HttpGet("ServiceOrders/{orderId}/Summary")]
    [Authorize]
    public async Task<IActionResult> GetOrderSummary(
        [FromRoute] int orderId,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.GetOrderSummaryAsync(orderId, userId, ct)).Respond();
    }

    [HttpGet("ServiceOrders/{orderId}")]
    [Authorize]
    public async Task<IActionResult> GetOrder(
        [FromRoute] int orderId,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.GetOrderAsync(orderId, userId, ct)).Respond();
    }

    [HttpGet("ServiceOrders")]
    [Authorize]
    public async Task<IActionResult> GetOrders(
        [FromQuery] ServiceOrderFilterRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.GetOrdersAsync(userId, request, ct)).Respond();
    }

    [HttpPut("ServiceOrders/{orderId}/Cancel")]
    [Authorize]
    public async Task<IActionResult> CancelOrder(
        [FromRoute] int orderId,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.CancelOrderAsync(orderId, userId, ct)).Respond();
    }

    [HttpPost("ServiceOrders/{orderId}/SubmitWorkAndMessage")]
    [Authorize]
    public async Task<IActionResult> SubmitWorkAndMessage(
        [FromRoute] int orderId,
        [FromForm] SubmitWorkAndMessageRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.SubmitWorkAndMessageAsync(orderId, userId, request, ct)).Respond();
    }

    [HttpGet("ServiceOrders/{orderId}/ConversationMessages")]
    [Authorize]
    public async Task<IActionResult> GetConversationMessages(
        [FromRoute] int orderId,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.GetConversationMessagesAsync(orderId, userId, ct)).Respond();
    }

    [HttpGet("ServiceOrders/Conversations")]
    [Authorize]
    public async Task<IActionResult> GetConversations(CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.GetConversationsAsync(userId, ct)).Respond();
    }

    #endregion

    #region Order End Operations

    [HttpPut("ServiceOrders/{orderId}/Complete")]
    [Authorize]
    public async Task<IActionResult> CompleteOrder(
        [FromRoute] int orderId,
        [FromBody] ReviewRequest request,
        CancellationToken ct)
    {
        return (await _serviceOrderService.CompleteOrderAsync(orderId, request, ct)).Respond();
    }

    [HttpPost("ServiceOrders/{orderId}/OpenDispute")]
    [Authorize]
    public async Task<IActionResult> OpenDispute(
        [FromRoute] int orderId,
        [FromBody] OrderDisputeRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        return (await _serviceOrderService.OpenDisputeAsync(orderId, userId, request, ct)).Respond();
    }

    #endregion
}
```

### 4. Data Layer

#### 4.1 Entities (Assuming they exist, verify and update if needed)

- `Service` entity
- `ServiceOrder` entity
- `ServiceOrderMessage` entity
- `ServiceOrderDeliverable` entity
- `ServiceReview` entity

#### 4.2 Repositories

- `IServiceRepository`
- `IServiceOrderRepository`

### 5. Dependency Injection

في `DependencyInjections.cs`:

```csharp
// Add new services
services.AddScoped<IServiceService, ServiceService>();
// Update existing registration if needed
services.AddScoped<IServiceOrderService, ServiceOrderService>();
```

## API Endpoints Summary

### Services API

```
GET    /api/Services                          - Get all services (with filtering)
GET    /api/Services/{id}                     - Get service details
POST   /api/Services                          - Add new service
PUT    /api/Services/{id}                     - Update service
DELETE /api/Services/{id}                     - Delete service
```

### ServiceOrder API

```
# Order Initialization
POST   /api/ServiceOrder/Services/{serviceId}/Orders                    - Create order
PUT    /api/ServiceOrder/Services/{serviceId}/Orders/{orderId}/Accept   - Accept order
PUT    /api/ServiceOrder/Services/{serviceId}/Orders/{orderId}/Reject   - Reject order
POST   /api/ServiceOrder/ServiceOrders/{orderId}/Payment                - Start payment

# Order Management
GET    /api/ServiceOrder/ServiceOrders                                  - Get user orders
GET    /api/ServiceOrder/ServiceOrders/{orderId}                        - Get order details
GET    /api/ServiceOrder/ServiceOrders/{orderId}/Summary                - Get order summary
PUT    /api/ServiceOrder/ServiceOrders/{orderId}/Cancel                 - Cancel order

# Communication
POST   /api/ServiceOrder/ServiceOrders/{orderId}/SubmitWorkAndMessage   - Submit work
GET    /api/ServiceOrder/ServiceOrders/{orderId}/ConversationMessages   - Get messages
GET    /api/ServiceOrder/ServiceOrders/Conversations                    - Get conversations

# Completion
PUT    /api/ServiceOrder/ServiceOrders/{orderId}/Complete               - Complete order
POST   /api/ServiceOrder/ServiceOrders/{orderId}/OpenDispute            - Open dispute
```

## Order State Machine

```
PendingProviderAcceptance → Accepted → PendingPayment → InProgress →
Delivered → Completed

Alternative flows:
- PendingProviderAcceptance → Rejected
- Any state → Cancelled (with conditions)
- InProgress/Delivered → Disputed
```

## Security Considerations

1. **Authorization**:
   - Service CRUD: Only service owner
   - Create Order: Authenticated customers
   - Accept/Reject Order: Only service provider
   - Cancel Order: Customer or Provider (with conditions)
   - Complete Order: Only customer
   - Open Dispute: Customer or Provider

2. **Validation**:
   - All inputs validated using FluentValidation
   - File uploads validated (size, type)
   - Business rules enforced (e.g., can't accept already accepted order)

3. **Data Protection**:
   - Sensitive data encrypted
   - User can only see their own orders
   - Payment details handled securely

## Error Handling

- Use `resultBase` for consistent responses
- Return appropriate HTTP status codes
- Provide clear error messages
- Log errors for debugging

## Performance Considerations

1. **Pagination**: All list endpoints support pagination
2. **Eager Loading**: Load related data efficiently
3. **Caching**: Consider caching for frequently accessed data
4. **Async/Await**: All operations are asynchronous
5. **File Handling**: Optimize image storage and retrieval

## Testing Strategy

1. **Unit Tests**: Test business logic in services
2. **Integration Tests**: Test API endpoints
3. **Manual Testing**: Test complete workflows
4. **Edge Cases**: Test error scenarios and edge cases

## Migration Path

1. Create new contracts
2. Create IServiceService interface
3. Implement ServiceService
4. Refactor IServiceOrderService interface
5. Refactor ServiceOrderService implementation
6. Create ServicesController
7. Refactor ServiceOrderController
8. Update DependencyInjections
9. Test each component
10. Deploy

## Notes

- Follow existing code patterns and conventions
- Maintain backward compatibility where possible
- Document any breaking changes
- Use same libraries and tools as Job system
- Keep code DRY (Don't Repeat Yourself)
