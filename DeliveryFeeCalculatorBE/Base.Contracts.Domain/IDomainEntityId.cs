namespace Base.Contracts.Domain;

public interface IDomainEntityId : IDomainEntityId<Guid>
{
    
}

public interface IDomainEntityId<Tkey>

    where Tkey : IEquatable<Tkey>
{
    public Tkey Id { get; set; }
    
}