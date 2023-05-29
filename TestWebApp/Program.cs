using Microsoft.EntityFrameworkCore;
using Serilog;
using TestWebApp.Data;
using TestWebApp.Filters;
using TestWebApp.Repositories;
using TestWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddScoped<UserResourceFilter>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskListService, TaskListService>();
builder.Services.AddScoped<IUserRepository, PostgresqlUserRepository>();
builder.Services.AddScoped<ITaskListRepository, PostgresqlTaskListRepository>();

builder.Services.AddLogging(loggerBuilder =>
{
    loggerBuilder.ClearProviders()
        .AddSerilog(new LoggerConfiguration()
            .WriteTo.File("logs/app.log")
            .CreateLogger());
});

var app = builder.Build();

InitDatabase(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

static void InitDatabase(IServiceProvider serviceProvider)
{
    using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    context?.Database.Migrate();
}
