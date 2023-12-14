using MakeMyCapServer.Services.Email;
using System.Text;
using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Distributors.MakeMyCap;


public class MakeMyCapOrderService : IOrderService
{
	public const string ORDER_EMAIL_ADDRESS = "orders@makemycap.com";

	private IEmailQueueService emailQueueService;
	private ILogger<MakeMyCapOrderService> logger;
	
	public MakeMyCapOrderService(IEmailQueueService emailQueueService, ILogger<MakeMyCapOrderService> logger)
	{
		this.emailQueueService = emailQueueService;
		this.logger = logger;
	} 
	
	public bool PlaceOrder(DistributorOrders orders)
	{
// TODO: CML - determine what additional formatting is needed here		
		var subject = $"Notification of sent PO {orders.PoNumber}";
		var body = new StringBuilder();
		body.Append($"The following order has been sent to {orders.DistributorName}\r\n\r\n");
		body.Append(OrderWriter.FormatOrder(orders));
		body.Append("\r\n");
			
		emailQueueService.Add(ORDER_EMAIL_ADDRESS, subject, body.ToString());
		return true;
	}
}