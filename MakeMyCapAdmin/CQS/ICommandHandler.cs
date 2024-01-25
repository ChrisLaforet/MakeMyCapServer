namespace MakeMyCapAdmin.CQS
{
	public interface ICommandHandler<in TCommand, out TResponse>
		where TCommand : ICommand
		where TResponse : class
	{
		TResponse Handle(TCommand command);
	}
}
