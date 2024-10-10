using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core.Messaging;

public interface ICommandBuss
{
    void Send<TCommand>(TCommand command) where TCommand : IBaseCommand;
    
}

public interface IBaseCommand
{
    bool Validate();
}


public class CommandBuss(IServiceProvider serviceProvider):ICommandBuss
{
    public void Send<TCommand>(TCommand command) where TCommand : IBaseCommand
    {
        var handler = serviceProvider.GetService<IBaseCommandHandler<TCommand>>();
        if (handler != null)
        {
            var validateHandler = new CommandValidatorIBaseCommandHandlerDecorator<TCommand>(handler);
            validateHandler.Handle(command);
        }
    }
}


public interface IBaseCommandHandler<in  TCommand> where TCommand : IBaseCommand
{
    void Handle(TCommand command);
}


public class CommandValidatorIBaseCommandHandlerDecorator<TCommand>(IBaseCommandHandler<TCommand> commandHandler):IBaseCommandHandler<TCommand> where TCommand:IBaseCommand
{
    public void Handle(TCommand command)
    {
        if (!command.Validate())
        {
            throw new Exception("Command is not valid");
        }
        commandHandler.Handle(command);
    }
}