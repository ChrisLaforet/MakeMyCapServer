using System.Security.Cryptography;
using System.Text;
using MakeMyCapServer.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ChurchMiceServer.Security.Auth;

public class PasswordProcessor
{
	const int ITERATIONS = 512;
	const string SALT = "NTE2NTUzNjg1NjZENTk3MQ==";
	const string NONCE = "lHGIbF86JHusugvSUUq10--877hshvGYjgsdmNNwdvjcZz029";

	private readonly string salt;
	private readonly string nonce;
	private int keyIterations;

	public PasswordProcessor(IConfigurationLoader configurationLoader)
	{
		this.salt = configurationLoader.GetKeyValueFor(Startup.SALT_STRING_KEY);
		this.nonce = configurationLoader.GetKeyValueFor(Startup.NONCE_STRING_KEY);
		this.keyIterations = int.Parse(configurationLoader.GetKeyValueFor(Startup.KEYITERATIONS_VALUE_KEY));
	}

	public PasswordProcessor(string salt, string nonce, int keyIterations)
	{
		this.salt = salt;
		this.nonce = nonce;
		this.keyIterations = keyIterations;
	}

	public string HashPassword(string password)
	{
		return Encrypt(CreateKey(password));
	}

	public string CreateKey(string password)
	{
		var key = KeyDerivation.Pbkdf2(password,
			//System.Convert.FromBase64String(salt),
			Encoding.UTF8.GetBytes(this.salt),
			prf: KeyDerivationPrf.HMACSHA512,
			iterationCount: ITERATIONS,
			numBytesRequested: 128 / 8);

		return System.Convert.ToBase64String(key);
	}

	public string Encrypt(string base64Key)
	{
		using (var aes = Aes.Create())
		{
			aes.KeySize = 128;
			aes.Key = Convert.FromBase64String(base64Key);
			aes.IV = new byte[16];
			aes.Mode = CipherMode.ECB;

			using (var memoryStream = new MemoryStream())
			{
				using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
				{
					byte[] plainBytes = Encoding.UTF8.GetBytes(this.nonce);
					cryptoStream.Write(plainBytes, 0, plainBytes.Length);
				}

				return Convert.ToBase64String(memoryStream.ToArray());
			}
		}
	}
}