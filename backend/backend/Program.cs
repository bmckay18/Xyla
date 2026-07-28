using Backend.Hubs;
using Backend.Mapping;
using Backend.Services.Lobbies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();

//Define my services
builder.Services.AddSingleton<ILobbyService, LobbyService>();

//Define mapping
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<LobbyMappingProfile>();
});

// Build app
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/gameHub");

app.Run();
