using ShopifyInventoryFulfillment.Configuration;
using ShopifyInventoryFulfillment.Services;
using ShopifyInventoryFulfillment.Shopify;

var builder = WebApplication.CreateBuilder(args);

var configurationPath = builder.Configuration.GetValue<string>("XmlConfiguration");
var xmlConfigurationLoader = new XmlConfigurationLoader(configurationPath);

// Add services to the container.
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

builder.Services.Add(new ServiceDescriptor(typeof(IConfigurationLoader), xmlConfigurationLoader));

// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IScopedProcessingService, InventoryUpdateService>();

builder.Services.AddHostedService<ScopedBackgroundService>();

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
