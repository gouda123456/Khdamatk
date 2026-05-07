using Khdamatk.Server;
using Khdamatk.Server.MiddleWares;
using Khdamatk.Server.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependancyInjections(builder.Configuration);

FileManagement.enableFileManagement(builder.Environment);


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.MapScalarApiReference();

}


app.UseHttpsRedirection();

app.UseCors();
app.UseMiddleware<GlobalErrorHandling>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
