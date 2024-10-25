using Riton_Task.Repositories;
using Microsoft.EntityFrameworkCore;
using Riton_Task.BackgroundServices;
using Riton_Task.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StaffDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("StaffDb")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IProcessedFilesRepository, ProcessedFilesRepository>();
builder.Services.AddHostedService<FileSaverBackgroundService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();