
﻿using Asp.Versioning;
using Khdamatk.Server.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

﻿using System.Text.Json.Serialization;
using Asp.Versioning;
using Khdamatk.Server.Helper.Payment;
using Microsoft.OpenApi.Models;
using Stripe;
using Stripe.BillingPortal;
using Microsoft.Extensions.DependencyInjection;


namespace Khdamatk.Server;

public static class DependancyInjections
{
    public static IServiceCollection AddDependancyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Database>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(
                configuration.GetConnectionString("YoussefFathy"),
                b => b.MigrationsAssembly(typeof(Database).Assembly.FullName)));

        services.AddHttpContextAccessor();
        services.AddAuthConfig(configuration);
        services.AddMapping();
        services.AddValidation();
        services.AddScoped<GlobalErrorHandling>();
        services.AddCORS();
        services.AddHttpClient();
        services.AddEmailHelper(configuration);
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                // لتحويل الـ Enums من وإلى String في الـ JSON
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddPaymentMethod(configuration);

        //services.AddStackExchangeRedisCache(options =>
        //{
        //    options.Configuration = configuration.GetConnectionString("RedisConnection");
        //});

#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        services.AddHybridCache();
#pragma warning restore EXTEXP0018

        services.AddAppServices();

        services.AddApiVersion();
        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        
        services.AddScoped<IAdminDashboardSerivce, AdminDashboardService>();
        services.AddScoped<IHomeService, HomeService>();

        services.AddScoped<IJobOrderService, JobOrderService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IServiceOrderService,ServiceOrderService>();

        services.AddScoped<IReportDashboardService, ReportDashboardService>();
        services.AddScoped<IServiceOrderService, ServiceOrderService>();
        services.AddScoped<IServiceProviderService, ServiceProviderService>();
        services.AddScoped<IServiceProviderService, ServiceProviderService>();
        services.AddScoped<IUserDashboardService, UserDashboardSerivce>();
        services.AddScoped<IPaymentService, PaymentService>();


        services.AddScoped<IRequestManagementDashboardSerivce, RequestManagementDashboardService>();
        services.AddScoped<IVerificationService, VerificationService>();
        services.AddScoped<IAdminVerificationService, AdminVerificationService>();
        services.AddScoped<IAdminReviewService, AdminReviewService>();
        services.AddScoped<IFinalDecisionService, FinalDecisionService>();

        return services;
    }

    public static IServiceCollection AddApiVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
        })
            .AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'V";
                
            });
        return services;
    }

    public static IServiceCollection AddPaymentMethod(this IServiceCollection services, IConfiguration configuration)
    {
        //Stripe
        services.AddOptions<StripeSetting>()
            .BindConfiguration(nameof(StripeSetting))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        StripeConfiguration.AppInfo = new AppInfo
        {
            Name = "Khdamatk API",
            Version = "1.0.0",
            Url = "https://khdamatk.com",
            PartnerId = "pp_partner_123456789"
        };
        StripeConfiguration.ApiKey = configuration.GetSection(nameof(StripeSetting)).Get<StripeSetting>()!.SecretKey;
        //services.AddScoped<TokenService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<PaymentIntentService>();
        services.AddScoped<RefundService>();
        services.AddScoped<ProductService>();
        services.AddScoped<SubscriptionService>();
        services.AddScoped<PriceService>();
        services.AddScoped<InvoiceService>();
        services.AddScoped<ChargeService>();
        services.AddScoped<RefundService>();
        services.AddScoped<SessionService>();


        //Fawaterak
        services.AddOptions<FawaterakSettings>()
            .BindConfiguration(nameof(FawaterakSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddTransient<IFawaterakPaymentHelper, FawaterakPaymentHelper>();



        services.AddScoped<IPaymentHelper, PaymentHelper>();

        return services;
    }

    public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration config)
    {

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<Database>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(op =>
        {
            op.User.RequireUniqueEmail = true;
            op.SignIn.RequireConfirmedEmail = true;
        });

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizeHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        services.AddOptions<JwtSetting>()
            .BindConfiguration(nameof(JwtSetting))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        JwtSetting? jwtSettings = config.GetSection(nameof(JwtSetting)).Get<JwtSetting>()!;


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Bearer
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero,

            };
        });


        services.AddScoped<ITokensService, TokensService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddAuthorization();
        return services;
    }


    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); ;

        return services;
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var configMapper = TypeAdapterConfig.GlobalSettings;
        configMapper.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(configMapper));


        return services;
    }

    public static IServiceCollection AddCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        return services;
    }
    public static IServiceCollection AddEmailHelper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ClientSetting>()
            .BindConfiguration(nameof(ClientSetting))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<EmailSetting>()
            .BindConfiguration(nameof(EmailSetting))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddTransient<IEmailHelper, EmailHelper>();

        return services;
    }

    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader();
        });


        return services;
    }

    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Khdamatk API", Version = "v1" });
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });
        return services;
    }
}

    
