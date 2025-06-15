namespace Ordering.Domain.Abstractions;

/// <summary>
/// Generic interface for IEntity
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}


public interface IEntity
{
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}
