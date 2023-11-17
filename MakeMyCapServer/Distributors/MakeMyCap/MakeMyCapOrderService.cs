using MakeMyCapServer.Services.Email;
using System.Text;
using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Distributors.MakeMyCap;


public class MakeMyCapOrderService : IOrderService
{
// TODO: CML - Important!  Update this Email address to Make My Cap Ordering Email		
	public const string ORDER_EMAIL_ADDRESS = "webmaster@makemycap.com";

	private IEmailQueueService emailQueueService;
	private ILogger<MakeMyCapOrderService> logger;
	
	public MakeMyCapOrderService(IEmailQueueService emailQueuServicee, ILogger<MakeMyCapOrderService> logger)
	{
		this.emailQueueService = emailQueueService;
		this.logger = logger;
	} 
	
	public bool PlaceOrder(IOrder order)
	{
// TODO: CML - determine what additional formatting is needed here		
		var subject = $"Notification of sent PO {order.PoNumber}";
		var body = new StringBuilder();
		body.Append($"The following order has been sent to {order.DistributorName}\r\n\r\n");
		body.Append(OrderWriter.FormatOrder(order));
		body.Append("\r\n");
		body.Append("Deliver to Decorated - Annex building, Cap America.\r\n\r\n");
			
		emailQueueService.Add(ORDER_EMAIL_ADDRESS, subject, body.ToString());
		return true;
	}
}