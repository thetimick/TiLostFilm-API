using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using TiLostFilm.Auth;
using TiLostFilm.Cache;
using TiLostFirm.Parser;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var apiVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "-";
    
    options.SwaggerDoc(
        "v1", 
        new OpenApiInfo 
        {
            Title = $"TiLostFilm - API (v.{apiVersion})"
        }
    );
    
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TiLostFilm.xml"));
    
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme());
    options.AddSecurityRequirement(new OpenApiSecurityRequirement());
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

// Auth

builder.Services
    .AddDbContext<AuthContext>(
        option => option.UseSqlite(builder.Configuration.GetConnectionString("AuthDataBaseConnection"))
    );

builder.Services
    .AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    )
    .AddJwtBearer();

builder.Services
    .AddAuthorization(
        options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        }
    );

builder.Services
    .AddIdentity<IdentityUser<long>, IdentityRole<long>>()
    .AddEntityFrameworkStores<AuthContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<AuthService>();

// Cache

builder.Services.AddDbContext<CacheContext>(
    option => option.UseSqlite(builder.Configuration.GetConnectionString("CacheDataBaseConnection"))
);

// Services

builder.Services.AddTransient<CacheService>();

builder.Services.AddTransient<ContentService>();
builder.Services.AddTransient<EpisodeService>();
builder.Services.AddTransient<MainService>();
builder.Services.AddTransient<SheduleService>();

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseRouting();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

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