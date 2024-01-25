
using MakeMyCapAdmin.Configuration;
using MakeMyCapAdmin.Controllers;
using MakeMyCapAdmin.Model;
using MakeMyCapAdmin.Proxies;
using MakeMyCapAdmin.Security;
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
builder.Services.AddDbContext<MakeMyCapServerContext>(options => options.UseSqlServer(xmlConfigurationLoader.GetKeyValueFor(DB_CONNECTION_STRING_KEY)));

builder.Services.AddScoped<IServiceProxy, ServiceProxy>();
builder.Services.AddScoped<IProductSkuProxy, ProductSkuProxy>();
builder.Services.AddScoped<IEmailProxy, EmailProxy>();
builder.Services.AddScoped<IOrderingProxy, OrderingProxy>();
builder.Services.AddScoped<IFulfillmentProxy, FulfillmentProxy>();
builder.Services.AddScoped<INotificationProxy, NotificationProxy>();
builder.Services.AddScoped<IUserProxy, UserProxy>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
	.AddAuthentication(LoginController.COOKIE_NAME)
	.AddCookie(LoginController.COOKIE_NAME, options =>
	{
		options.Cookie.Name = LoginController.COOKIE_NAME;
		options.LoginPath = "/Login";
	});

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

app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
