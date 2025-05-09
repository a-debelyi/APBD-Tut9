using APBD_8.Middlewares;
using APBD_9.Repositories;
using APBD_9.Services;
using APBD_9.Services.Finders;
using APBD_9.Services.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductsWarehouseService, ProductsWarehouseService>();
builder.Services.AddScoped<IProductsWarehouseRepository, ProductsWarehouseRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IWarehousesRepository, WarehousesRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IOrderFinder, OrderFinder>();
builder.Services.AddScoped<IResourceValidator, ResourceValidator>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();