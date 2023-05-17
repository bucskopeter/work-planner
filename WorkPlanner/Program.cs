using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using WorkPlanner.Data;
using WorkPlanner.Domain;
using WorkPlanner.Domain.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddData(builder.Configuration);
builder.Services.AddDomain();

builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "WorkPlanner API",
        Description = "A compact API designed for planning of worker shifts."
    });

    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WorkPlanner.xml"));
});

builder.Services.AddAutoMapper(typeof(WorkerProfile));

var app = builder.Build();

await app.Services.RunMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
