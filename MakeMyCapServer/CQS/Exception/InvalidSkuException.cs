namespace MakeMyCapServer.CQS.Exception;

public class InvalidSkuException : System.Exception
{
	public InvalidSkuException(string sku) : base($"Cannot find sku {sku}") {}

}