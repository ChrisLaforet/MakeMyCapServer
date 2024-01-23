namespace MakeMyCapServer.CQS.Query;

public class SkuQuery : IQuery
{
	public string Sku { get; }

	public SkuQuery(string sku)
	{
		Sku = sku;
	}
}