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

builder.Services.AddSingleton<LostFilmDataBase>();

builder.Services.AddSingleton<ContentService>();
builder.Services.AddSingleton<EpisodeService>();
builder.Services.AddSingleton<MainService>();
builder.Services.AddSingleton<SheduleService>();

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