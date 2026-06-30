using Khdamatk.Server.Contracts.Service;

namespace Khdamatk.Server.Services.Implementations;

public class ServiceService : IServiceService
{
    private readonly Database _db;
    private readonly IMapper _mapper;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(
        Database db,
        IMapper mapper,
        ILogger<ServiceService> logger)
    {
        _db = db;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Adding new service with title: {Title}", request.Title);

            // 1. Validate provider exists
            var provider = await _db.ServiceProviderProfiles
                .FirstOrDefaultAsync(p => p.UserId == request.ProviderProfileId, ct);

            if (provider == null)
            {
                _logger.LogWarning("Provider not found: {ProviderId}", request.ProviderProfileId);
                return Failure(StatusCodes.Status404NotFound, 
                    FailureMessages.DataNotFound.Title, 
                    "Service provider not found.");
            }

            // 2. Validate category exists
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Name == request.CategoryName, ct);

            if (category == null)
            {
                category = new Category()
                {
                    Name = request.CategoryName,
                    Description = $"Category for {request.CategoryName}",
                    Icon = "fa-solid fa-arrow-up-from-water-pump",
                    IsActive = true
                };
                await _db.Categories.AddAsync(category, ct);
                await _db.SaveChangesAsync(ct);
            }

            // 3. Create service entity
            var service = new Service
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                DetailedDescription = request.DetailedDescription,
                Price = request.Price,
                DeliveryTimeInDays = request.DeliverTimeInDays,
                RevisionCount = request.RevisionCount,
                Concepts = request.Concepts,
                CategoryId = category.Id,
                ServiceProviderProfileId = request.ProviderProfileId,
                IsActive = true,
                IsApproved = true,
                CreatedAt = DateTime.UtcNow,
                AverageRating = 0,
                TotalReviews = 0,
                SalesCount = 0,
                ViewCount = 0
            };

            // 4. Process main image if provided
            if (request.ServiceEnvelope != null)
            {
                // TODO: Process and save main image
                // This would typically involve saving the file and creating a Media entity
                _logger.LogInformation("Main image provided for service");
            }

            // 5. Process attachments if provided
            if (request.Attachment != null && request.Attachment.Count > 0)
            {
                // TODO: Process and save attachments
                // This would typically involve saving files and creating ServiceMedia entities
                _logger.LogInformation("Attachments provided: {Count}", request.Attachment.Count);
            }

            // 6. Save to database
            await _db.Services.AddAsync(service, ct);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Service created successfully with ID: {ServiceId}", service.Id);

            // 7. Return service details
            var response = await GetServiceAsync(service.Id, ct);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding service with title: {Title}", request.Title);
            return Failure(StatusCodes.Status500InternalServerError, 
                FailureMessages.General.Title, 
                "An error occurred while adding the service.");
        }
    }

    public async Task<resultBase> GetCategoriesServicesAsync(string CategoryName, CancellationToken ct = default)
    {
        var response = await _db.Categories.Select(c => new
        {
            CategoryId = c.Id,
            CategoryName = c.Name,
            ServicesCount = c.Services.Count()
        }).ToListAsync();
        return Success(StatusCodes.Status200OK, response);
    }

    public async Task<resultBase> GetServiceAsync(int serviceId, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Getting service with ID: {ServiceId}", serviceId);

            var service = await _db.Services
                .Include(s => s.Category)
                .Include(s => s.ServiceProviderProfile)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.ProfilePicture)
                .Include(s => s.MainImage)
                .Include(s => s.MediaGalleryLinks)
                    .ThenInclude(m => m.Media)
                .Include(s => s.Orders)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDelete, ct);

            if (service == null)
            {
                _logger.LogWarning("Service not found: {ServiceId}", serviceId);
                return Failure(StatusCodes.Status404NotFound, 
                    FailureMessages.DataNotFound.Title, 
                    "Service not found.");
            }

            // Increment view count
            var serviceToUpdate = await _db.Services.FindAsync(new object[] { serviceId }, ct);
            if (serviceToUpdate != null)
            {
                serviceToUpdate.ViewCount++;
                await _db.SaveChangesAsync(ct);
            }

            // Map to response
            var response = new ServiceDetailsResponse(
                service.Id,
                service.Title,
                service.ShortDescription,
                service.DetailedDescription,
                service.Price,
                service.RevisionCount,
                service.DeliveryTimeInDays,
                ExperienceLevel.Entry, // TODO: Get from service or provider
                service.Concepts,
                service.MainImage != null ? File.ReadAllBytes(service.MainImage.FullPath) : Array.Empty<byte>(),
                service.MediaGalleryLinks.Select(m => File.ReadAllBytes(m.Media.FullPath)).ToList(),
                service.Orders.Count,
                service.AverageRating,
                new ProviderServiceInfo(
                    service.ServiceProviderProfileId,
                    service.ServiceProviderProfile.User.FullName ?? service.ServiceProviderProfile.User.UserName ?? "Unknown",
                    service.ServiceProviderProfile.JobTitle ?? "Service Provider",
                    service.ServiceProviderProfile.User.ProfilePicture != null 
                        ? File.ReadAllBytes(service.ServiceProviderProfile.User.ProfilePicture.FullPath) 
                        : Array.Empty<byte>(),
                    service.ServiceProviderProfile.AverageRating,
                    service.ServiceProviderProfile.AverageResponseTime,
                    service.ServiceProviderProfile.TotalReviews
                )
            );

            _logger.LogInformation("Service retrieved successfully: {ServiceId}", serviceId);
            return Success(StatusCodes.Status200OK, 
                SuccessMessages.General.Title, 
                "Service retrieved successfully.", 
                response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service: {ServiceId}", serviceId);
            return Failure(StatusCodes.Status500InternalServerError, 
                FailureMessages.General.Title, 
                "An error occurred while retrieving the service.");
        }
    }

    public async Task<resultBase> GetServiceAsync(string serviceName, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Getting service with ID: {serviceName}", serviceName);

            var service = await _db.Services
                .Include(s => s.Category)
                .Include(s => s.ServiceProviderProfile)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.ProfilePicture)
                .Include(s => s.MainImage)
                .Include(s => s.MediaGalleryLinks)
                    .ThenInclude(m => m.Media)
                .Include(s => s.Orders)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Title == serviceName && !s.IsDelete, ct);

            if (service == null)
            {
                _logger.LogWarning("Service not found: {serviceName}", serviceName);
                return Failure(StatusCodes.Status404NotFound,
                    FailureMessages.DataNotFound.Title,
                    "Service not found.");
            }

            // Increment view count
            var serviceToUpdate = await _db.Services.FindAsync(new object[] { serviceName }, ct);
            if (serviceToUpdate != null)
            {
                serviceToUpdate.ViewCount++;
                await _db.SaveChangesAsync(ct);
            }

            // Map to response
            var response = new ServiceDetailsResponse(
                service.Id,
                service.Title,
                service.ShortDescription,
                service.DetailedDescription,
                service.Price,
                service.RevisionCount,
                service.DeliveryTimeInDays,
                ExperienceLevel.Entry, // TODO: Get from service or provider
                service.Concepts,
                service.MainImage != null ? File.ReadAllBytes(service.MainImage.FullPath) : Array.Empty<byte>(),
                service.MediaGalleryLinks.Select(m => File.ReadAllBytes(m.Media.FullPath)).ToList(),
                service.Orders.Count,
                service.AverageRating,
                new ProviderServiceInfo(
                    service.ServiceProviderProfileId,
                    service.ServiceProviderProfile.User.FullName ?? service.ServiceProviderProfile.User.UserName ?? "Unknown",
                    service.ServiceProviderProfile.JobTitle ?? "Service Provider",
                    service.ServiceProviderProfile.User.ProfilePicture != null
                        ? File.ReadAllBytes(service.ServiceProviderProfile.User.ProfilePicture.FullPath)
                        : Array.Empty<byte>(),
                    service.ServiceProviderProfile.AverageRating,
                    service.ServiceProviderProfile.AverageResponseTime,
                    service.ServiceProviderProfile.TotalReviews
                )
            );

            _logger.LogInformation("Service retrieved successfully: {serviceName}", serviceName);
            return Success(StatusCodes.Status200OK,
                SuccessMessages.General.Title,
                "Service retrieved successfully.",
                response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service: {serviceName}", serviceName);
            return Failure(StatusCodes.Status500InternalServerError,
                FailureMessages.General.Title,
                "An error occurred while retrieving the service.");
        }
    }

    public async Task<resultBase> GetServicesAsync(ServiceFilterRequest request, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Getting services with filters");

            // Build query
            var query = _db.Services
                .Include(s => s.Category)
                .Include(s => s.ServiceProviderProfile)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.ProfilePicture)
                .Include(s => s.MainImage)
                .Include(s => s.Orders)
                .Where(s => !s.IsDelete && s.IsActive && s.IsApproved)
                .AsNoTracking()
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                query = query.Where(s => 
                    s.Title.ToLower().Contains(searchTerm) ||
                    s.ShortDescription.ToLower().Contains(searchTerm) ||
                    s.Concepts.Any(c => c.ToLower().Contains(searchTerm)));
            }

            // Apply category filter
            if (!string.IsNullOrWhiteSpace(request.CategoryName))
            {
                query = query.Where(s => s.Category.Name == request.CategoryName);
            }

            // Apply price filters
            if (request.MinPrice.HasValue)
            {
                query = query.Where(s => s.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(s => s.Price <= request.MaxPrice.Value);
            }

            // Apply delivery time filters
            if (request.MinDeliveryDays.HasValue)
            {
                query = query.Where(s => s.DeliveryTimeInDays >= request.MinDeliveryDays.Value);
            }

            if (request.MaxDeliveryDays.HasValue)
            {
                query = query.Where(s => s.DeliveryTimeInDays <= request.MaxDeliveryDays.Value);
            }

            // Apply rating filter
            if (request.MinRating.HasValue)
            {
                query = query.Where(s => s.AverageRating >= request.MinRating.Value);
            }

            if(request.ServiceProviderId != null)
            {
                query = query.Where(s => s.ServiceProviderProfileId == request.ServiceProviderId);
            }

            if(request.UserId != null)
            {
                query = query.Where(s => s.ServiceProviderProfile.UserId == request.UserId);
            }

            // Apply sorting
            query = request.SortBy?.ToLower() switch
            {
                "price" => request.SortDescending 
                    ? query.OrderByDescending(s => s.Price) 
                    : query.OrderBy(s => s.Price),
                "rating" => request.SortDescending 
                    ? query.OrderByDescending(s => s.AverageRating) 
                    : query.OrderBy(s => s.AverageRating),
                "sales" => request.SortDescending 
                    ? query.OrderByDescending(s => s.SalesCount) 
                    : query.OrderBy(s => s.SalesCount),
                "deliverytime" => request.SortDescending 
                    ? query.OrderByDescending(s => s.DeliveryTimeInDays) 
                    : query.OrderBy(s => s.DeliveryTimeInDays),
                _ => request.SortDescending 
                    ? query.OrderByDescending(s => s.CreatedAt) 
                    : query.OrderBy(s => s.CreatedAt)
            };

            // Get total count
            var totalCount = await query.CountAsync(ct);

            // Apply pagination
            var services = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(ct);

            // Map to response
            var response = services.Select(s => new ServiceSummaryResponse(
                s.Id,
                s.Title,
                s.ShortDescription,
                s.Price,
                s.MainImage != null ? File.ReadAllBytes(s.MainImage.FullPath) : Array.Empty<byte>(),
                s.Orders.Count,
                s.AverageRating,
                s.DeliveryTimeInDays,
                new ProviderSummaryInfo(
                    s.ServiceProviderProfileId,
                    s.ServiceProviderProfile.User.FullName ?? s.ServiceProviderProfile.User.UserName ?? "Unknown",
                    s.ServiceProviderProfile.User.ProfilePicture != null 
                        ? File.ReadAllBytes(s.ServiceProviderProfile.User.ProfilePicture.FullPath) 
                        : Array.Empty<byte>(),
                    s.ServiceProviderProfile.AverageRating
                ),
                (s.ServiceProviderProfile.User.VerificationData != null) ? $"{s.ServiceProviderProfile.User.VerificationData.Country},{s.ServiceProviderProfile.User.VerificationData.City}" : "N/A",
                s.IsActive,
                s.CreatedAt
            )).ToList();

            _logger.LogInformation("Retrieved {Count} services out of {Total}", services.Count, totalCount);

            return Success(StatusCodes.Status200OK, 
                SuccessMessages.General.Title, 
                "Services retrieved successfully.", 
                new { Services = response, TotalCount = totalCount, PageNumber = request.PageNumber, PageSize = request.PageSize });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting services");
            return Failure(StatusCodes.Status500InternalServerError, 
                FailureMessages.General.Title, 
                "An error occurred while retrieving services.");
        }
    }

    public async Task<resultBase> UpdateServiceAsync(int serviceId, UpdateServiceRequest request, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Updating service: {ServiceId}", serviceId);

            // 1. Get existing service
            var service = await _db.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDelete, ct);

            if (service == null)
            {
                _logger.LogWarning("Service not found: {ServiceId}", serviceId);
                return Failure(StatusCodes.Status404NotFound, 
                    FailureMessages.DataNotFound.Title, 
                    "Service not found.");
            }

            // 2. Validate category if changed
            if (request.CategoryName != service.Category.Name)
            {
                var category = await _db.Categories
                    .FirstOrDefaultAsync(c => c.Name == request.CategoryName, ct);

                if (category == null)
                {
                    _logger.LogWarning("Category not found: {CategoryName}", request.CategoryName);
                    return Failure(StatusCodes.Status404NotFound, 
                        FailureMessages.DataNotFound.Title, 
                        "Category not found.");
                }

                service.CategoryId = category.Id;
            }

            // 3. Update properties
            service.Title = request.Title;
            service.ShortDescription = request.ShortDescription;
            service.DetailedDescription = request.DetailedDescription;
            service.Price = request.Price;
            service.DeliveryTimeInDays = request.DeliverTimeInDays;
            service.RevisionCount = request.RevisionCount;
            service.Concepts = request.Concepts;
            service.UpdatedAt = DateTime.UtcNow;

            // 4. Process new images if provided
            if (request.ServiceEnvelope != null)
            {
                // TODO: Process and update main image
                _logger.LogInformation("New main image provided for service: {ServiceId}", serviceId);
            }

            // 5. Process new attachments if provided
            if (request.Attachment != null && request.Attachment.Count > 0)
            {
                // TODO: Process and update attachments
                _logger.LogInformation("New attachments provided for service: {ServiceId}", serviceId);
            }

            // 6. Save changes
            _db.Services.Update(service);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Service updated successfully: {ServiceId}", serviceId);

            // 7. Return updated service
            return await GetServiceAsync(serviceId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service: {ServiceId}", serviceId);
            return Failure(StatusCodes.Status500InternalServerError, 
                FailureMessages.General.Title, 
                "An error occurred while updating the service.");
        }
    }

    public async Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Deleting service: {ServiceId}", serviceId);

            // 1. Get service
            var service = await _db.Services
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDelete, ct);

            if (service == null)
            {
                _logger.LogWarning("Service not found: {ServiceId}", serviceId);
                return Failure(StatusCodes.Status404NotFound, 
                    FailureMessages.DataNotFound.Title, 
                    "Service not found.");
            }

            // 2. Check if service has active orders
            var hasActiveOrders = service.Orders.Any(o => 
                o.Status == OrderStatus.Pending ||
                o.Status == OrderStatus.PendingPayment ||
                o.Status == OrderStatus.Active);

            if (hasActiveOrders)
            {
                _logger.LogWarning("Cannot delete service with active orders: {ServiceId}", serviceId);
                return Failure(StatusCodes.Status400BadRequest, 
                    FailureMessages.Conflict.Title, 
                    "Cannot delete service with active orders.");
            }

            // 3. Soft delete
            service.IsDelete = true;
            service.UpdatedAt = DateTime.UtcNow;

            _db.Services.Update(service);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation("Service deleted successfully: {ServiceId}", serviceId);

            return Success(StatusCodes.Status200OK, 
                SuccessMessages.General.Title, 
                "Service deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service: {ServiceId}", serviceId);
            return Failure(StatusCodes.Status500InternalServerError, 
                FailureMessages.General.Title, 
                "An error occurred while deleting the service.");
        }
    }

    public async Task<resultBase> GetProviderServicesAsync(string providerId, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Getting services for provider: {ProviderId}", providerId);

            var services = await _db.Services
                .Include(s => s.Category)
                .Include(s => s.ServiceProviderProfile)
                    .ThenInclude(p => p.User)
                        .ThenInclude(u => u.ProfilePicture)
                .Include(s => s.MainImage)
                .Include(s => s.Orders)
                .Where(s => s.ServiceProviderProfileId == providerId && !s.IsDelete)
                .AsNoTracking()
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync(ct);

            var response = services.Select(s => new ServiceSummaryResponse(
                s.Id,
                s.Title,
                s.ShortDescription,
                s.Price,
                s.MainImage != null ? File.ReadAllBytes(s.MainImage.FullPath) : Array.Empty<byte>(),
                s.Orders.Count,
                s.AverageRating,
                s.DeliveryTimeInDays,
                new ProviderSummaryInfo(
                    s.ServiceProviderProfileId,
                    s.ServiceProviderProfile.User.FullName ?? s.ServiceProviderProfile.User.UserName ?? "Unknown",
                    s.ServiceProviderProfile.User.ProfilePicture != null 
                        ? File.ReadAllBytes(s.ServiceProviderProfile.User.ProfilePicture.FullPath) 
                        : Array.Empty<byte>(),
                    s.ServiceProviderProfile.AverageRating
                ),
                (s.ServiceProviderProfile.User.VerificationData != null) ? $"{s.ServiceProviderProfile.User.VerificationData.Country},{s.ServiceProviderProfile.User.VerificationData.City}" : "N/A",
                s.IsActive,
                s.CreatedAt
            )).ToList();

            _logger.LogInformation("Retrieved {Count} services for provider: {ProviderId}", services.Count, providerId);

            return Success(StatusCodes.Status200OK, 
                SuccessMessages.General.Title, 
                "Provider services retrieved successfully.", 
                response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting services for provider: {ProviderId}", providerId);
            return Failure(StatusCodes.Status500InternalServerError, 
                FailureMessages.General.Title, 
                "An error occurred while retrieving provider services.");
        }
    }



    public async Task<resultBase> GetCategoryNameServices(string CategoryName, CancellationToken ct = default)
    {
        var respose = await _db.Services
            .Where(s => s.Category.Name == CategoryName && !s.IsDelete)
            .Select(s => new ServiceSummaryResponse(
                s.Id,
                s.Title,
                s.ShortDescription,
                s.Price,
                s.MainImage != null ? File.ReadAllBytes(s.MainImage.FullPath) : Array.Empty<byte>(),
                s.Orders.Count,
                s.AverageRating,
                s.DeliveryTimeInDays,
                new ProviderSummaryInfo(
                    s.ServiceProviderProfileId,
                    s.ServiceProviderProfile.User.FullName ?? s.ServiceProviderProfile.User.UserName ?? "Unknown",
                    s.ServiceProviderProfile.User.ProfilePicture != null
                        ? File.ReadAllBytes(s.ServiceProviderProfile.User.ProfilePicture.FullPath)
                        : Array.Empty<byte>(),
                    s.ServiceProviderProfile.AverageRating
                ),
                (s.ServiceProviderProfile.User.VerificationData != null) ? $"{s.ServiceProviderProfile.User.VerificationData.Country},{s.ServiceProviderProfile.User.VerificationData.City}" : "N/A",
                s.IsActive,
                s.CreatedAt
            )).ToListAsync();

        return Success(StatusCodes.Status200OK, respose);
    }



}
