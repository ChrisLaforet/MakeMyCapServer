namespace MakeMyCapServer.Distributors.Exceptions;

public class MissingDataException : Exception
{
	public MissingDataException(string reason) : base(reason) {}
}