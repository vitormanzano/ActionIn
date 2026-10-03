namespace ActionIn.Core.Messages.Commands;

public interface ICommand : IBaseCommand
{
}

public interface ICommand<TResponse> : IBaseCommand { }


