using MakeMyCapServer.Model;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services;
using MakeMyCapServer.Services.Background;
using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Services.Fulfillment;
using MakeMyCapServer.Services.Inventory;
using MakeMyCapServer.Services.OrderPlacement;
using MakeMyCapServer.Shopify;
using Microsoft.EntityFrameworkCore;

const string DB_CONNECTION_STRING_KEY = "MakeMyCapDatabase";

var builder = WebApplication.CreateBuilder(args);

var configurationPath = builder.Configuration.GetValue<string>("XmlConfiguration");
var xmlConfigurationLoader = new XmlConfigurationLoader(configurationPath);

// Add services to the container.
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

builder.Services.Add(new ServiceDescriptor(typeof(IConfigurationLoader), xmlConfigurationLoader));

// create db contexts for each of the domains
builder.Services.AddDbContext<MakeMyCapServerContext>(options =>
					options.UseSqlServer(xmlConfigurationLoader.GetKeyValueFor(DB_CONNECTION_STRING_KEY)));

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryProcessingService, InventoryUpdateService>();
builder.Services.AddScoped<IFulfillmentProcessingService, FulfillmentUpdateService>();
builder.Services.AddScoped<IOrderPlacementProcessingService, OrderPlacementQueueService>();
builder.Services.AddScoped<IEmailService, SendgridEmailService>();
builder.Services.AddScoped<(IEmailQueue, EmailQueue)>();
builder.Services.AddScoped<IDistributorServiceLookup, DistributorServiceLookup>();

builder.Services.AddScoped<IServiceProxy, ServiceProxy>();
builder.Services.AddScoped<IProductSkuProxy, ProductSkuProxy>();

builder.Services.AddHostedService<InventoryScopedBackgroundService>();
builder.Services.AddHostedService<FulfillmentScopedBackgroundService>();
builder.Services.AddHostedService<OrderPlacementScopedBackgroundService>();

builder.Services.AddHttpClient();

// builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	// app.UseExceptionHandler("/Home/Error");
	// // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	// app.UseHsts();
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
