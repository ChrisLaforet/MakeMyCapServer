using System.Text;
using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Distributors.SanMar;

public class SanMarOrderService : IOrderService
{
	private const char COMMA = ',';
 

	public bool PlaceOrder(IOrder order)
	{
// TODO: CML - finish implementing this		
throw new NotImplementedException();
	}
	
	private string SanitizeString(string value)
	{
		// SanMar states: Please Note: Do Not Use Additional Commas in any Field Due to the Comma being our Delimiter in order files.
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}

		if (value.Contains(COMMA))
		{
			var sanitized = new StringBuilder();
			foreach (char ch in value)
			{
				if (ch != COMMA)
				{
					sanitized.Append(value);
				}
			}
			return sanitized.ToString().Trim();
		}

		return value.Trim();
	}

}