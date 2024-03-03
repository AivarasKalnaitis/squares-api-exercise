using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog.Events;
using Serilog;
using Squares.Business;
using Squares.Infrastructure.Persistance;
using Squares.Infrastructure.Persistance.Repositories;


var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog();


builder.Services.AddDbContext<DataContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly("Squares.API")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPointsListService, PointsListService>();
builder.Services.AddScoped<IPointsListRepository, PointsListRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SquaresAPI v1"));


app.UseRouting();
app.UseHttpMetrics();
app.UseMetricServer();

app.MapMetrics();
app.MapControllers();

app.Run();