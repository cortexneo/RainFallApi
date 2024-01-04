using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using RainFallApi.Data;
using RainFallApi.Domain;
using RainFallApi.Presentation.Helpers;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHttpClient("RainFallApiBaseUrl", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("RainfallApi:BaseUrl"));
    c.DefaultRequestHeaders.Accept.Clear();
    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddScoped(typeof(IRainFallReaderService), typeof(RainFallReaderService));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Rainfall Api",
        Version = "1.0",
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com")
        },
        Description = "An API which provides rainfall reading data"
    });
    options.AddServer(new OpenApiServer
    {
        Url = "http://localhost:3000",
        Description = "Rainfall API"
    });
    options.DocumentFilter<TagDescriptionsDocumentFilter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.EnableAnnotations();
});

var app = builder.Build();

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
