using CompraProgramada.Application.SharedContext.Results;
using CompraProgramada.Application.SharedContext.UseCases.Abstractions;
using CompraProgramada.Domain.ClientContext.Entities;
using CompraProgramada.Domain.ClientContext.Repositories;
using CompraProgramada.Domain.ClientContext.ValueObjects;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Application.ClientContext.Commands.ProductAdherence;

public class ProductAdherenceCommandHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository) : ICommandHandler<ProductAdherenceCommand>
{
    public async Task<Result> Handle(ProductAdherenceCommand request, CancellationToken cancellationToken)
    {
        var cpf = new Cpf(request.Cpf); 
        var client = await clientRepository.GetClientByCpfAsync(cpf.Number);
        if (client is not null)
            return Result.Failure(new Error("400", "CPF already registered in the system."));

        var email = new Email(request.Email);
        var monthlyValue = new MonthlyValue(request.MonthlyValue);

        var newClient = Client.Create(request.Name, cpf, email, monthlyValue);

        await clientRepository.AddClientAsync(newClient);
        await unitOfWork.CommitAsync(cancellationToken);
        
        return Result.Create(new ProductAdherenceCommandResponse(newClient));
    }
}