using System.Security.Cryptography.Pkcs;
using System.Text;
using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Distributors.CapAmerica;

public class CapAmericaOrderService : IOrderService
{
// TODO: CML - Important!  Update this Email address to CapAmerica Ordering Email		
	public const string ORDER_EMAIL_ADDRESS = "webmaster@makemycap.com";

	private IEmailQueue emailQueue;
	private ILogger<CapAmericaInventoryService> logger;
	
	public CapAmericaOrderService(IEmailQueue emailQueue, ILogger<CapAmericaInventoryService> logger)
	{
		this.emailQueue = emailQueue;
		this.logger = logger;
	} 
	
	public bool PlaceOrder(IOrder order)
	{
// TODO: CML - determine what additional formatting is needed here		
		var subject = $"MakeMyCap PO {order.PoNumber}";
		var body = new StringBuilder();
		body.Append("Customer: Make My Cap");
		body.Append(OrderWriter.FormatOrder(order));
		body.Append("\r\n");
		body.Append("Deliver to Decorated - Annex building, Cap America.\r\n\r\n");
			
		emailQueue.Add(ORDER_EMAIL_ADDRESS, subject, body);
	}
}