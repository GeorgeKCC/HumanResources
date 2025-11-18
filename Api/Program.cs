using ColaboratorModule;
using LoginModule;
using ManagementModule;
using Shared;
using Shared.hub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
RegisterServiceShared(builder);
builder.Services.RegisterColaboratorServices();
builder.Services.RegisterManagementServices();
builder.Services.RegisterLoginService();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ApplicationSharedModule();

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications").RequireAuthorization();

app.Run();

static void RegisterServiceShared(WebApplicationBuilder builder)
{
    SharedService.BuilderSharedModule(builder);
    builder.Services.ServicesSharedModule(builder.Configuration);
}