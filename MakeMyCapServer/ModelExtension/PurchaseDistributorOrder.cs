using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Model;

public partial class PurchaseDistributorOrder : IDistributorOrder
{
	public DateTime OrderDate => CreateDate;
	
	public string PoNumber => Ponumber;

	public string DistributorName
	{
		get
		{
			if (!string.IsNullOrEmpty(Distributor.Name))
			{
				return Distributor.Name;
			}

			return string.Empty;
		}
	}
}
