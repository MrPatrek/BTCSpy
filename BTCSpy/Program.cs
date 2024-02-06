using BTCSpy;
using BTCSpy.ActionFilters;
using BTCSpy.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureServiceManager();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;     // disable default model state validation ('cause we will have our custom one)
                                                        // i.e., disable automatic HTTP 400 – Bad Request responses
                                                        // (e.g., when we send empty body request to the API when it is expected to be non-empty)
});

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddControllers()
    .AddApplicationPart(typeof(BTCSpy.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandler(opt => { });

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
