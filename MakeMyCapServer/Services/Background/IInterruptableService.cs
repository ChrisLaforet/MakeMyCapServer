namespace MakeMyCapServer.Services.Background;

public interface IInterruptableService
{
	void ResumeProcessingNow();
}