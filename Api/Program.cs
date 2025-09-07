using ColaboratorModule;
using Shared;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
RegisterServiceShared(builder);
RegisterServiceColaborator(builder);

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterServiceColaborator(WebApplicationBuilder builder)
{
    builder.Services.RegisterColaboratorServices(builder.Configuration);
}

static void RegisterServiceShared(WebApplicationBuilder builder)
{
    SharedService.BuilderSharedModule(builder);
    builder.Services.ServicesSharedModule();
}