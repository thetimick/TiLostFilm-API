using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TiLostFilm.Cache;
using TiLostFilm.Cache.Context;
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

builder.Services.AddDbContext<CacheContext>(
    option => option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddTransient<CacheService>();

builder.Services.AddTransient<ContentService>();
builder.Services.AddTransient<EpisodeService>();
builder.Services.AddTransient<MainService>();
builder.Services.AddTransient<SheduleService>();

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseRouting();
app.MapControllers();
app.MapGet("/", () => "TiLostFilm API\nSwagger: /swagger");

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
      
app.Run();