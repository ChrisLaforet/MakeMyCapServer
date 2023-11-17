namespace MakeMyCapServer.Lookup.Exceptions;

public class ServiceNotFoundException : Exception
{
	public ServiceNotFoundException(string reason) : base(reason) {}
}