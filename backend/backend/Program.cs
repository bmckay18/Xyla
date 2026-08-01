using Backend.CustomExceptions;
using Backend.Hubs;
using Backend.Mapping;
using Backend.Services.Lobbies;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policies
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();

// Define custom services
builder.Services.AddSingleton<ILobbyService, LobbyService>();

// Define automapping
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<LobbyMappingProfile>();
});

// Build app
var app = builder.Build();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "text/plain";

        if (exception is UnauthorisedException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(exception.Message);
            return;
        }
        else if (exception is BadRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(exception.Message);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(exception?.Message ?? "An internal server error occurred");
        return;

    });
});

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/gameHub");

app.Run();