using Tzather.Identity.Api.Models;
using Tzather.Identity.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<ITokenService>(option => new JwtTokenService(new IdentityModel())); // Add identity service

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGet("/", (HttpResponse response) =>
{
  response.ContentType = "text/html";
  return @"<html><body style='padding:100px 0;text-align:center;font-size:xxx-large;'>
  Api is running<br/><br/>
  <a href='/swagger'>View Swagger</a>
</body></html>";
}).WithTags("/");
app.MapControllers();

app.Run();
