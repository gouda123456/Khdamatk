using Khdamatk.Server;
using Khdamatk.Server.MiddleWares;
using Khdamatk.Server.Services;
using Scalar.AspNetCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependancyInjections(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
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
