using CompraProgramada.Application.SharedContext.Results;
using MediatR;

namespace CompraProgramada.Application.SharedContext.UseCases.Abstractions;

public interface ICommand : IRequest<Result>;