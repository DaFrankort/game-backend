using Server.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Server.Services.LobbyService>();
builder.Services.AddSingleton<Server.Services.UserService>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
