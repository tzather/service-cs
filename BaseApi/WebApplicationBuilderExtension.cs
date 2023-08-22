using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tzather.BaseApi;

public static class WebApplicationBuilderExtension
{

  public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder, BaseAppSettings appSettings)
  {
    var services = builder.Services;
    services
      .AddAuthentication()
      .AddJwtBearer("Bearer", option => new JwtTokenService(appSettings.Identity).Configure(option));

    services.AddCors(options =>
      options.AddPolicy(appSettings.CorsOrigin, builder =>
        builder
          .WithOrigins(appSettings.CorsOrigin)
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
      )
    );
    builder.Services.AddSingleton<ILoggerProvider>(n => new DatabaseLoggerProvider(appSettings.Name, appSettings.LogDbContext));

    services.AddControllers(options =>
    {
      options.Filters.Add(new AuthorizeFilter());
      options.Filters.Add(typeof(ActionFilter));
      // options.Filters.Add(typeof(ExceptionFilter));
      // // Add Global OutputFormatters 
      options.Filters.Add(new ProducesAttribute("application/json", "application/xml", "text/csv"));
      options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
      options.OutputFormatters.Add(new CsvFormatter());

    })
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
    services.AddSwaggerGen(options => AddSwagger(options, appSettings.Name, appSettings.Version));
    services.AddScoped<ITokenService>(option => new JwtTokenService(appSettings.Identity)); // Add identity service
    return builder;
  }

  public static WebApplication BuildApp(this WebApplicationBuilder builder, BaseAppSettings appSettings)
  {

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/v{appSettings.Version}/swagger.json", $"{appSettings.Name} v{appSettings.Version}"));
    app.UseCors(appSettings.CorsOrigin);
    app.UseHttpsRedirection();
    app.UseAuthentication();
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
    return app;
  }

  public static void AddDatabase<ITContext, TContext>(this WebApplicationBuilder builder, IServiceCollection services, string connectionString)
    where ITContext : class
    where TContext : DbContext, ITContext
  {
    services.AddDbContext<TContext>(options => options
      .UseSqlServer(connectionString)
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // don't track entities
      .EnableSensitiveDataLogging() // log sql param values
    );
    services.AddScoped<ITContext, TContext>();
  }
  private static void AddSwagger(SwaggerGenOptions options, string title, string version)
  {
    options.SwaggerDoc($"v{version}", new OpenApiInfo
    {
      Title = $"{title} Api",
      Version = version
    });
    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml");
    if (File.Exists(xmlPath))
    {
      options.IncludeXmlComments(xmlPath);
    }

    // AddSecurityDefinition and AddSecurityRequirement needed to allow users to pass in jwt token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
      In = ParameterLocation.Header,
      Description = "Please insert JWT with Bearer into field",
      Name = "Authorization",
      Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {{
        new OpenApiSecurityScheme{
          Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          }
        },
        new string[] { }
        }});
    options.CustomSchemaIds(x => x.Name.Replace("Model", "")); // Display the model without the word "Model"
    options.EnableAnnotations();
  }
}

