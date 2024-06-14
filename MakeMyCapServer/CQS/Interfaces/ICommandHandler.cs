namespace MakeMyCapServer.CQS.Interfaces
{
	public interface ICommandHandler<in TCommand, out TResponse>
		where TCommand : ICommand
		where TResponse : class
	{
		TResponse Handle(TCommand command);
	}
}
