using CompraProgramada.Application.SharedContext.Results;
using MediatR;

namespace CompraProgramada.Application.SharedContext.UseCases.Abstractions;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand;