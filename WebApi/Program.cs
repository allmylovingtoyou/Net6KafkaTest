using WebApi.Clients;
using WebApi.Mappings;
using WebApi.Services;
using WebApi.Validators;

var builder = WebApplication.CreateBuilder(args);

//Add envs
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();

// Add validators
builder.Services.AddTransient<DepartureTimeChangeDtoValidator>();

// Add automapper and mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<DepartureTimeChangeMapper>();

//Add services
builder.Services.AddScoped<IDepartureService, DepartureService>();
builder.Services.AddScoped<IKafkaClient, KafkaClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();