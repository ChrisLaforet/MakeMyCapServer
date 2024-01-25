using System.Security.Cryptography;
using System.Text;
using MakeMyCapAdmin.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MakeMyCapAdmin.Security.Auth;

public class PasswordProcessor
{
	private const string SALT_STRING_KEY = "Salt";
	private const string NONCE_STRING_KEY = "Nonce";
	private const string KEYITERATIONS_VALUE_KEY = "KeyIterations";

	private readonly string salt;
	private readonly string nonce;
	private int keyIterations;

	public PasswordProcessor(IConfigurationLoader configurationLoader)
	{
		this.salt = configurationLoader.GetKeyValueFor(SALT_STRING_KEY);
		this.nonce = configurationLoader.GetKeyValueFor(NONCE_STRING_KEY);
		this.keyIterations = int.Parse(configurationLoader.GetKeyValueFor(KEYITERATIONS_VALUE_KEY));
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
			iterationCount: keyIterations,
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