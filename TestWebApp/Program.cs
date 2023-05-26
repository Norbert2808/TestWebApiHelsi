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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
