using Microsoft.OpenApi.Models;
using TiLostFilm.DataBase;
using TiLostFirm.Parser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TiLostFilm - API",
        Description = "API для проекта TiLostFilm",
        Version = "1.0",
        Contact = new OpenApiContact
        {
            Name = "TheTimickRus",
            Url = new Uri("https://github.com/TheTimickRus")
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

builder.Services.AddSingleton<DataBase>();

builder.Services.AddSingleton<ContentService>();
builder.Services.AddSingleton<EpisodeService>();
builder.Services.AddSingleton<MainService>();
builder.Services.AddSingleton<SheduleService>();

/* ======= [ Main ] ======= */

// Build APP
var app = builder.Build();

// Cors
app.UseCors("CorsPolicy");

app.UseRouting();
app.MapControllers();

app.MapGet("/", () => "TiLostFilm API\nSwagger: /swagger");

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TiWeWatch - API V1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "TiWeWatch - API V2");
});
      
app.Run();

/* ======= [ Main ] ======= */