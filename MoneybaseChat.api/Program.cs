using Microsoft.EntityFrameworkCore;
using MoneybaseChat.infrastructure.persistence;
using MoneybaseChat.infrastructure.ioc;
using MoneybaseChat.infrastructure.workers;
using MoneybaseChat.infrastructure.queue.rabbitmq;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// process settings file
builder.Services.Configure<RabbitMqServerSettings>(builder.Configuration.GetSection("RabbitMQ"));

// setup DBContext
builder.Services.AddDbContext<MoneybaseChatContext>(opt =>
    opt.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// add services from infrastructure 
builder.Services.AddInfrastructureServices();
// add service from application
builder.Services.AddApplicationServices();

// add service workers
builder.Services.AddHostedService<QueueWorker>();
builder.Services.AddHostedService<StartupWorker>();
builder.Services.AddHostedService<PulseMonitor>();
builder.Services.AddControllers();

var app = builder.Build();


// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MoneybaseChatContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();
app.Run();
