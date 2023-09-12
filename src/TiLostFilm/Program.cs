using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using TiLostFilm.Cache;
using TiLostFilm.Cache.Context;
using TiLostFirm.Parser;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

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
    
    var filePath = Path.Combine(AppContext.BaseDirectory, "TiLostFilm.xml");
    options.IncludeXmlComments(filePath);
});

builder.Services.AddSwaggerGenNewtonsoftSupport();

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

// Swagger
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
app.UseReDoc(c =>
{
    c.RoutePrefix = "redoc";
});
app.UseEndpoints(endpoints =>
{
    endpoints.Map("/", context => Task.Run(() => context.Response.Redirect("/swagger")));
});

app.Run();