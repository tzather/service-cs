using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tzather.BaseApi;

public static class WebApplicationBuilderExtension
{

  public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder, string name, string version, string corsOrigin, IdentityModel identityModel)
  {
    var services = builder.Services;
    services
      .AddAuthentication()
      .AddJwtBearer("Bearer", option => new JwtTokenService(identityModel).Configure(option));

    services.AddCors(options =>
      options.AddPolicy(corsOrigin, builder =>
        builder
          .WithOrigins(corsOrigin)
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
      )
    );

    services.AddControllers(options =>
    {
      options.Filters.Add(new AuthorizeFilter());
      options.Filters.Add(typeof(ActionFilter));
    })
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
    services.AddSwaggerGen(options => AddSwagger(options, name, version));
    services.AddScoped<ITokenService>(option => new JwtTokenService(identityModel)); // Add identity service
    return builder;
  }

  public static WebApplication BuildApp(this WebApplicationBuilder builder, string name, string version, string corsOrigin)
  {
    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"{name} v{version}"));
    app.UseCors(corsOrigin);
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

