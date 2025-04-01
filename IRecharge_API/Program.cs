using IRecharge_API.BLL;
using IRecharge_API.DAL;
using IRecharge_API.ExternalServices;
using IRecharge_API.ExternalServices.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Dbcontext
builder.Services.AddSwaggerGen();

// Dbcontext
var connectionstrings = builder.Configuration.GetConnectionString("IRechargeDb");
builder.Services.AddDbContext<IRechargeDbContext>(options =>
{
    options.UseSqlServer(connectionstrings);
});

// Register HttpClient
builder.Services.AddHttpClient("DigitalVendorsUrl", client => {
    client.BaseAddress = new Uri("https://api3.digitalvendorz.com/api/");
}
);

builder.Services.AddTransient<IPurchaseService, PurchaseService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IDigitalVendors, DigitalVendorsAPI>();

builder.Services.AddScoped<AirtimeService>();
builder.Services.AddSingleton<TokenServices>();
builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
