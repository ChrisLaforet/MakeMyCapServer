using System.Security.Cryptography.Pkcs;
using System.Text;
using MakeMyCapServer.Orders;
using MakeMyCapServer.Services.Email;

namespace MakeMyCapServer.Distributors.CapAmerica;

public class CapAmericaOrderService : IOrderService
{
// TODO: CML - Important!  Update this Email address to CapAmerica Ordering Email		
	public const string ORDER_EMAIL_ADDRESS = "webmaster@makemycap.com";

	private IEmailQueueService emailQueueService;
	private ILogger<CapAmericaInventoryService> logger;
	
	public CapAmericaOrderService(IEmailQueueService emailQueueService, ILogger<CapAmericaInventoryService> logger)
	{
		this.emailQueueService = emailQueueService;
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
			
		emailQueueService.Add(ORDER_EMAIL_ADDRESS, subject, body.ToString());
		return true;
	}
}