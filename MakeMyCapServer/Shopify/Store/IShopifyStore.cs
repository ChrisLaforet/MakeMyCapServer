﻿namespace MakeMyCapServer.Shopify;

public interface IShopifyStore
{
	string StoreName { get; }
	string DomainName { get; }
	string BaseUrl { get; }
	string Location { get; }
	string ApiToken { get; }
}