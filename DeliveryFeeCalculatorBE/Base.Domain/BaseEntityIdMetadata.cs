using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace Base.Domain;

public abstract class BaseEntityIdMetadata : BaseEntityIdMetadata<Guid>
{
    
}

public abstract class BaseEntityIdMetadata<TKey> : BaseEntityId<TKey>, IDomainEntityMetadata
    where TKey : IEquatable<TKey>
{
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

    [MaxLength(128)]
    public string UpdatedBy { get; set; } = default!;
    public DateTime UpdatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
}