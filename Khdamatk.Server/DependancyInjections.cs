using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace Khdamatk.Server;

public static class DependancyInjections
{
    public static IServiceCollection AddDependancyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Database>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("menna"),
                b => b.MigrationsAssembly(typeof(Database).Assembly.FullName)));

        services.AddHttpContextAccessor();
        services.AddAuthConfig(configuration);
        services.AddMapping();
        services.AddValidation();
        services.AddScoped<GlobalErrorHandling>();
        services.AddCORS();
        services.AddEmailHelper(configuration);



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

        var jwtSettings = config.GetSection(nameof(JwtSetting)).Get<JwtSetting>()!;


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

    
