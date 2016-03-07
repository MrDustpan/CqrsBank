using System;
using System.Threading.Tasks;

namespace CqrsBank.Infrastructure
{
  public interface IAsyncCommandHandler<in TCommand> where TCommand : ICommand
  {
    Task HandleAsync(TCommand command);
  }

  public interface IAsyncCommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
  {
    Task<TResult> HandleAsync(TCommand command);
  }

  public interface IAsyncQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
  {
    Task<TResult> HandleAsync(TQuery query);
  }

  public interface ICommand { }

  // ReSharper disable once UnusedTypeParameter
  public interface ICommand<TResult> { }

  public interface ICommandHandler<in TCommand> where TCommand : ICommand
  {
    void Handle(TCommand command);
  }

  public interface ICommandHandler<in TCommand, out TResult> where TCommand : ICommand<TResult>
  {
    TResult Handle(TCommand command);
  }

  public interface ICommandProcessor
  {
    void Process(ICommand command);
    Task ProcessAsync(ICommand command);
    TResult Process<TResult>(ICommand<TResult> command);
    Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command);
  }

  // ReSharper disable once UnusedTypeParameter
  public interface IQuery<TResult> { }

  public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
  {
    TResult Handle(TQuery query);
  }

  public interface IQueryProcessor
  {
    TResult Process<TResult>(IQuery<TResult> query);
    Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query);
  }

  public class CommandProcessor : ICommandProcessor
  {
    private readonly SingleInstanceFactory _factory;

    public CommandProcessor(SingleInstanceFactory factory)
    {
      _factory = factory;
    }

    public void Process(ICommand command)
    {
      var handlerType =
            typeof(ICommandHandler<>).MakeGenericType(command.GetType());

      dynamic handler = _factory(handlerType);

      handler.Handle((dynamic)command);
    }

    public Task ProcessAsync(ICommand command)
    {
      var handlerType =
            typeof(IAsyncCommandHandler<>).MakeGenericType(command.GetType());

      dynamic handler = _factory(handlerType);

      return handler.HandleAsync((dynamic)command);
    }

    public TResult Process<TResult>(ICommand<TResult> command)
    {
      var handlerType =
            typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

      dynamic handler = _factory(handlerType);

      return handler.Handle((dynamic)command);
    }

    public Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command)
    {
      var handlerType =
            typeof(IAsyncCommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

      dynamic handler = _factory(handlerType);

      return handler.HandleAsync((dynamic)command);
    }
  }

  public class QueryProcessor : IQueryProcessor
  {
    private readonly SingleInstanceFactory _factory;

    public QueryProcessor(SingleInstanceFactory factory)
    {
      _factory = factory;
    }

    public TResult Process<TResult>(IQuery<TResult> query)
    {
      var handlerType =
            typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

      dynamic handler = _factory(handlerType);

      return handler.Handle((dynamic)query);
    }

    public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
    {
      var handlerType =
            typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

      dynamic handler = _factory(handlerType);

      return handler.HandleAsync((dynamic)query);
    }
  }

  public delegate object SingleInstanceFactory(Type serviceType);
}