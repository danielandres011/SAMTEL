using Microsoft.EntityFrameworkCore;
using RESERVAS.API.Data;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(x =>
{
    x.UseSqlServer(configuration["ConnectionStrings:DBConnection"]);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API SAMTEL RESERVAS",
        Description = "Una API web ASP.NET Core para administrar Reservas de Hoteles.",
        TermsOfService = new Uri("http://samtel.co"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "SAMTEL",
            Url = new Uri("http://samtel.co")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "Licencia de Uso",
            Url = new Uri("http://samtel.co")
        }
    });
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
