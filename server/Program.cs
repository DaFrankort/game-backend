using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Filters;
using Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Server.Services.LobbyService>();
builder.Services.AddSingleton<Server.Services.UserService>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuthMiddleware>();

app.MapControllers();
app.Run();
