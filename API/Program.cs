using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Core.Interfaces;
using Microsoft.Data.Sqlite;
using API.Helpers;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using API.Controllers.Errors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<StoreContext>(x => x.UseSqlite(connectionString));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
        .Where(e => e.Value.Errors.Count > 0)
        .SelectMany(e => e.Value.Errors)
        .Select(e => e.ErrorMessage).ToArray();

        var errorResponse = new ApiValidationErrorResponse
        {
            ErrorMessages = errors
        };

        return new BadRequestObjectResult(errorResponse);
    };
});

//builder.Services.AddScoped( _ => new SqliteConnection(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//var logger = app.Services.GetRequiredService<ILoggerFactory>();

await EnsureDb(app.Services, app.Logger);

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task EnsureDb(IServiceProvider services, ILogger logger)
{ 
    logger.LogInformation("Ensuring database exists at connection string '[connectionString]'", connectionString);
 
    try
    {
         var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<StoreContext>();
         await context.Database.MigrateAsync();
         await StoreContextSeed.SeedAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during migration");
    }
}