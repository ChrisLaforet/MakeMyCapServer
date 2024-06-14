namespace MakeMyCapServer.Security.JWT;

public class JWTDecodeException : Exception
{
	public JWTDecodeException(string message) : base(message) { }
	public JWTDecodeException(string message, Exception cause) : base(message, cause) { }
}

