using MakeMyCapServer.Model;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Distributors;
using MakeMyCapServer.Distributors.PurchaseOrder;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Security;
using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Services.Fulfillment;
using MakeMyCapServer.Services.Inventory;
using MakeMyCapServer.Services.Notification;
using MakeMyCapServer.Services.OrderPlacement;
using MakeMyCapServer.Shopify.Services;
using MakeMyCapServer.Shopify.Store;
using MakeMyCapServer.Webhooks;
using Microsoft.EntityFrameworkCore;
using IInventoryService = MakeMyCapServer.Shopify.Services.IInventoryService;
using IOrderService = MakeMyCapServer.Shopify.Services.IOrderService;

const string DB_CONNECTION_STRING_KEY = "MakeMyCapDatabase";

const string ALLOW_SPECIFIC_ORIGINS = "AllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var configurationPath = builder.Configuration.GetValue<string>("XmlConfiguration");
var xmlConfigurationLoader = new XmlConfigurationLoader(configurationPath);

// Add services to the container.
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

builder.Services.Add(new ServiceDescriptor(typeof(IConfigurationLoader), xmlConfigurationLoader));

// create db contexts for each of the domains
builder.Services.AddDbContext<MakeMyCapServerContext>(options => options.UseSqlServer(xmlConfigurationLoader.GetKeyValueFor(DB_CONNECTION_STRING_KEY)));

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderGenerator, PurchaseOrderCreator>();

builder.Services.AddScoped<IInventoryProcessingService, InventoryUpdateService>();
builder.Services.AddScoped<IFulfillmentProcessingService, FulfillmentUpdateService>();
builder.Services.AddScoped<IOrderPlacementProcessingService, OrderPlacementQueueService>();
builder.Services.AddScoped<IEmailQueueProcessingService, EmailQueueProcessingService>();

builder.Services.AddScoped<IEmailSender, SendgridEmailSender>();
builder.Services.AddScoped<IEmailQueueService, EmailQueueService>();
builder.Services.AddScoped<IDistributorServiceLookup, DistributorServiceLookup>();
builder.Services.AddScoped<IShopifyStore, ShopifyStore>();

builder.Services.AddScoped<IServiceProxy, ServiceProxy>();
builder.Services.AddScoped<IProductSkuProxy, ProductSkuProxy>();
builder.Services.AddScoped<IEmailProxy, EmailProxy>();
builder.Services.AddScoped<IOrderingProxy, OrderingProxy>();
builder.Services.AddScoped<INotificationProxy, NotificationProxy>();
builder.Services.AddScoped<IUserProxy, UserProxy>();

builder.Services.AddScoped<IStatusNotificationService, StatusNotificationService>();

// TODO: CML - uncomment once finished development testing with React UI!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// builder.Services.AddHostedService<InventoryScopedBackgroundService>();
// builder.Services.AddHostedService<FulfillmentScopedBackgroundService>();
// builder.Services.AddHostedService<OrderPlacementScopedBackgroundService>();
// builder.Services.AddHostedService<EmailSendingScopedBackgroundService>();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<ShopifyWebhookService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: ALLOW_SPECIFIC_ORIGINS,
		policy  =>
		{
			policy.WithOrigins("http://localhost:3000")
				.AllowAnyHeader()
				//.WithMethods("POST", "PUT", "GET", "OPTIONS")
				.AllowAnyMethod()
				.AllowCredentials();
		});
});

builder.Services.AddAuthentication(options =>
	{
		// Set the default authentication scheme
		options.DefaultAuthenticateScheme = "MakeMyCapScheme";
		options.DefaultChallengeScheme = "MakeMyCapScheme";
	})
	.AddScheme<MakeMyCapAuthenticationOptions, MakeMyCapAuthenticationHandler>("MakeMyCapScheme", options => { });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	// app.UseExceptionHandler("/Home/Error");
	// // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	// app.UseHsts();
	app.UseDeveloperExceptionPage();
}

app.UseCors(ALLOW_SPECIFIC_ORIGINS);

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<ValidateClientApiTokenMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapPost("/shopify/orderCreated", async (ShopifyWebhookService ws, HttpContext context) => await ws.AcceptOrderCreatedNotification(context));

try
{
	app.Run();
}
catch (Exception ex)
{
	Console.WriteLine("Exception caught running MMC:");
	Console.WriteLine(ex);
	Console.WriteLine(ex.StackTrace);
}
