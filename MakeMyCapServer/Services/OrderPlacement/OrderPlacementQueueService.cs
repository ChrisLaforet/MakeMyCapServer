using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;

namespace MakeMyCapServer.Services.OrderPlacement;

public class OrderPlacementQueueService : IOrderPlacementProcessingService
{
	private const int FIVE_MINUTES = 5;
	private const int PROCESSING_TIMEOUT_MSEC = FIVE_MINUTES * 60 * 1000;

	private const int WARNING_HOURS = 2;
	private const int FAILURE_HOURS = 48;

	private readonly IServiceProxy serviceProxy;
	private readonly IOrderingProxy orderingProxy;
	private readonly ILogger<OrderPlacementQueueService> logger;
	private readonly IEmailService emailService;

	public OrderPlacementQueueService(IServiceProxy serviceProxy, IOrderingProxy orderingProxy, IEmailService emailService, ILogger<OrderPlacementQueueService> logger)
	{
		this.serviceProxy = serviceProxy;
		this.orderingProxy = orderingProxy;
		this.logger = logger;
		this.emailService = emailService;
	}
	
	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(OrderPlacementQueueService));
		bool firstTime = true;
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!ProcessOrderQueue())
			{
				await Task.Delay(PROCESSING_TIMEOUT_MSEC, stoppingToken);
			}
		}
	}

	private bool ProcessOrderQueue()
	{
		logger.LogInformation("Checking for queued orders that need sending");

		var pendingPurchaseOrders = orderingProxy.GetPendingPurchaseOrders();
		if (pendingPurchaseOrders.Count == 0)
		{
			logger.LogInformation("Nothing to do.");
			return false;
		}

		foreach (var pendingPurchaseOrder in pendingPurchaseOrders)
		{
			if (AttemptToTransmitPurchaseOrder(pendingPurchaseOrder))
			{
				pendingPurchaseOrder.SuccessDateTime = DateTime.Now;
			}

			pendingPurchaseOrder.Attempts += 1;
			orderingProxy.SavePurchaseOrder(pendingPurchaseOrder);
		}
	}

	private bool AttemptToTransmitPurchaseOrder(PurchaseOrder purchaseOrder)
	{
		try
		{

		}
		catch (Exception ex)
		{
			logger.LogError($"Exception caught while attempting to send PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id}: {ex}");
			HandleNotificationsFor(purchaseOrder);
		}
		
	}

	private void HandleNotificationsFor(PurchaseOrder purchaseOrder)
	{
		var now = DateTime.Now;
		if (now.CompareTo(purchaseOrder.CreateDate.AddHours(FAILURE_HOURS)) >= 0)
		{
			logger.LogError($"PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id} has expired the Failure time after {purchaseOrder.Attempts} attempts to deliver!");
			TransmitErrorMessage(PurchaseOrder purchaseOrder);
			return;
		}

		var difference = now.Subtract(purchaseOrder.CreateDate);
		int expectedAlertCount = difference.Hours / WARNING_HOURS;

		if (expectedAlertCount < purchaseOrder.Attempts)
		{
			logger.LogWarning($"PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id} has not transmitted after {purchaseOrder.Attempts} attempts to deliver!");
			TransmitWarningMessage(purchaseOrder);
		}
	}

	private void TransmitErrorMessage(PurchaseOrder purchaseOrder)
	{
		try
		{
			var recipients = serviceProxy.GetCriticalEmailRecipients();
			// prepare body
			
			// ship email
			
			purchaseOrder.FailureNotificationDateTime = now;

		}
		catch (Exception ex)
		{
			
		}
	}
	
	
}