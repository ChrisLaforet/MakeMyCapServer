namespace MakeMyCapServer.Webhooks;

public class ShopifyWebhookService
{
	public async Task AcceptOrderCreateNotification(HttpContext context)
	{
		var requestBody = await context.Request.ReadFromJsonAsync<MakeMyCapServer.Shopify.Dtos.Fulfillment.Order>();
		Console.WriteLine("Got traffic");
		context.Response.StatusCode = 200;
		await context.Response.WriteAsync("Ack");
	}
}