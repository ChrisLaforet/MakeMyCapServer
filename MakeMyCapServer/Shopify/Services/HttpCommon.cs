using System.Text;
using MakeMyCapServer.Shopify.Store;

namespace MakeMyCapServer.Shopify.Services;

public static class HttpCommon
{
	public static Task<HttpResponseMessage> SendRequest(HttpMethod httpMethod, string requestUri, IShopifyStore shopifyStore, string jsonBodyContent = null)
	{
		var client = new HttpClient();
		var request = new HttpRequestMessage(httpMethod, requestUri);
		request.Headers.Add("X-Shopify-Access-Token", shopifyStore.ApiToken);
		request.Headers.Add("Accept", "application/json");
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");
		if (!string.IsNullOrEmpty(jsonBodyContent))
		{
			request.Content = new StringContent(jsonBodyContent,
				Encoding.UTF8, 
				"application/json");
		}
		return client.SendAsync(request);
	}
}