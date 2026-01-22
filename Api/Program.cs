using HumanResourcesApiAggregate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Add services host custom
builder.Host.RegisterHost();

// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.RegisterApp();

app.Run();