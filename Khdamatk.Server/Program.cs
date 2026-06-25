using System.Text.Json;
using Khdamatk.Server;
using Khdamatk.Server.MiddleWares;
using Khdamatk.Server.Services;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.MaxDepth = 64; // الافتراضي 32
});


builder.Services.AddOpenApi(options =>
{
    // 1. إضافة تعريف الـ JWT على مستوى المستند بالكامل
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "أدخل توكين الـ JWT الخاص بك هنا مباشرة (بدون كلمة Bearer)"
        });
        return Task.CompletedTask;
    });

    // 2. تفعيل قفل الأمان فقط على الـ Endpoints التي تحمل صفة [Authorize]
    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        // التحقق مما إذا كان الـ Endpoint محميًا بـ AuthorizeAttribute
        var hasAuthorize = context.Description.ActionDescriptor.EndpointMetadata
            .OfType<AuthorizeAttribute>().Any();

        if (hasAuthorize)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
        return Task.CompletedTask;
    });
});

builder.Services.AddDependancyInjections(builder.Configuration);


builder.Services.AddMapster();

var app = builder.Build();
FileManagement.enableFileManagement(builder.Environment);

// 1. أول حاجة الـ Error Handling عشان يلحق أي مصيبة تحصل
app.UseMiddleware<GlobalErrorHandling>();

app.UseDefaultFiles();
app.UseStaticFiles();

// 2. الترتيب المهم جداً للـ Security
app.UseHttpsRedirection();
app.UseRouting(); // ضيف دي صراحة عشان تضمن الترتيب
app.UseCors();

app.UseAuthentication(); // لازم Authentication الأول (مين إنت؟)
app.UseAuthorization();  // بعدين Authorization (مسموح لك تعمل إيه؟)

// 3. الـ UI بتاع التيست (Swagger/Scalar)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // تأكد إن دي موجودة
    app.UseSwaggerUI();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();
