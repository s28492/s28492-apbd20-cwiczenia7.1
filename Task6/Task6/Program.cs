using Task6.Repositories;
using Task6.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Własne buildery
builder.Services.AddControllers();
builder.Services.AddScoped<IProduct_WarehouseRepository, Product_WarehouseRepository>();
builder.Services.AddScoped<IProduct_WarehouseService, Product_WarehouseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Własny MapControllers
app.MapControllers();

app.Run();

