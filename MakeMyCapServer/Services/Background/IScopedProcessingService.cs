namespace MakeMyCapServer.Services.Background;


public interface IScopedProcessingService
{
	Task DoWorkAsync(CancellationToken stoppingToken);
}