var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
// }
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
});
app.MapControllers();

app.Run();
