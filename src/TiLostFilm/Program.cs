using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using TiLostFilm.DataBase;
using TiLostFirm.Parser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddApiVersioning(config =>
// {
//     config.DefaultApiVersion = new ApiVersion(1,0);
//     config.AssumeDefaultVersionWhenUnspecified = true;
//     config.ReportApiVersions = true;
//     config.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
// });
//
// builder.Services.AddVersionedApiExplorer(config =>
// {
//     config.GroupNameFormat = "'v'VVV";
//     config.SubstituteApiVersionInUrl = true;
// });

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

builder.Services.AddSingleton<LostFilmDataBase>();
builder.Services.AddSingleton<LostFilmParser>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();