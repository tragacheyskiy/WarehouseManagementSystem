using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Data.Repositories;
using WarehouseManagementSystem.Domain.CommandHandlers;
using WarehouseManagementSystem.Domain.CommandHandlers.Abstractions;
using WarehouseManagementSystem.Domain.Commands;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.Domain.QueryHandlers;
using WarehouseManagementSystem.Domain.QueryHandlers.Abstractions;
using WarehouseManagementSystem.Domain.Services;
using WarehouseManagementSystem.Domain.Services.Abstractions;
using WarehouseManagementSystem.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options => options.SchemaGeneratorOptions.SupportNonNullableReferenceTypes = true)
    .AddDbContext<AppDbContext>((serviceProvider, options) =>
    {
        var dbOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<DbOptions>>().Value;
        options.UseNpgsql(dbOptions.ConnectionString);
    })
    .Configure<DbOptions>(options => builder.Configuration.GetRequiredSection(nameof(DbOptions)).Bind(options))
    .AddSingleton<IDateTimeProvider, DateTimeProvider>()
    .AddScoped<IGetProductService, ProductRepository>()
    .AddScoped<IGetWarehouseService, WarehouseRepository>()
    .AddScoped<IGetWarehouseByIdService, WarehouseRepository>()
    .AddScoped<IUpdateWarehouseService, WarehouseRepository>()
    .AddScoped<ICreateProductsTransferService, ProductsTransferRepository>()
    .AddScoped<IGetProductsTransferService, ProductsTransferRepository>()
    .AddScoped<IQueryHandler<GetWarehouseReportQuery, WarehouseReport?>, GetWarehouseReportQueryHandler>()
    .AddScoped<ICommandHandler<TransferProductsCommand, ProductsTransfer?>, TransferProductsCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
