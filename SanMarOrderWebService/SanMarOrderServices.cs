using SanMarOrderService;

namespace SanMarOrderWebService;

public class SanMarOrderServices
{
	private readonly string customerNumber;
	private readonly string userName;
	private readonly string password;

	public SanMarOrderServices(string customerNumber, string userName, string password)
	{
		this.customerNumber = customerNumber;
		this.userName = userName;
		this.password = password;
	}

	public async Task<SanMarOrderResponse> SubmitPurchaseOrder(SanMarOrderRequest request)
	{
		using (var client = new SanMarOrderService.SanMarPOServiceDelegateClient())
		{
			var user = new SanMarOrderService.webServiceUser();
			user.sanMarCustomerNumber = customerNumber;
			user.sanMarUserName = userName;
			user.sanMarUserPassword = password;

			var details = new List<webServicePODetail>();
			foreach (var requestDetail in request.Details)
			{
				var detail = new webServicePODetail();
				detail.style = requestDetail.Style;
				if (!string.IsNullOrEmpty(requestDetail.Size))
				{
					detail.size = requestDetail.Size;
				}
				if (!string.IsNullOrEmpty(requestDetail.Color))
				{
					detail.color = requestDetail.Color;
				}
				detail.quantity = requestDetail.Quantity;
				
				details.Add(detail);
			}
			
			var order = new SanMarOrderService.webServicePO();
			order.attention = request.Attention;
			order.poNum = request.PoNumber;
			order.shipTo = request.ShipTo;
			order.shipAddress1 = request.Address1;
			if (!string.IsNullOrEmpty(request.Address2))
			{
				order.shipAddress2 = request.Address2;
			}

			order.shipCity = request.City;
			order.shipState = request.State;
			order.shipZip = request.Zip;
			order.webServicePoDetailList = details.ToArray();
			
			var response = await client.submitPOAsync(order, user);
			if (response == null || response.@return == null)
			{
				return new SanMarOrderResponse() {Success = false, Message = "Empty response returned from SanMar"};
			}

			return new SanMarOrderResponse() {Success = !response.@return.errorOccurred, Message = response.@return.message};
		}
	}
}