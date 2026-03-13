var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Server.Services.LobbyService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
