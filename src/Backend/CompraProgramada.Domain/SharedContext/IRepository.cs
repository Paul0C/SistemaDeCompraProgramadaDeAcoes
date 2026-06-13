namespace CompraProgramada.Domain.SharedContext;

public interface IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot;