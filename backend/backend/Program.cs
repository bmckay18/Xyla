using Backend.Hubs;
using Backend.Mapping;
using Backend.Services.Lobbies;

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

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/gameHub");

app.Run();