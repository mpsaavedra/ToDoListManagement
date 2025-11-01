using Bootler.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Data;

/// <summary>
/// Base entity implementation that contains auditable information and
/// domain event registry
/// </summary>
public abstract class Entity : IEntity, IEquatable<Entity>
{
    private long _id;
    private bool _softDeleted = false;
    private string? _rowVersion = Guid.NewGuid().ToString();
    private readonly List<IDomainEvent> _domainEvents = new();
    private long _createdBy;
    private DateTime _createdAt;
    private DateTime? _lastUpdatedAt;
    private long? _lastUpdatedBy;

    /// <summary>
    /// <inheritdoc cref="IAuditableEntity.CreatedBy"/>
    /// </summary>
    public long CreatedBy
    {
        get => _createdBy;
        set => _createdBy = value;
    }

    /// <summary>
    /// <inheritdoc cref="IAuditableEntity.CreatedAt"/>
    /// </summary>
    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value;
    }

    /// <summary>
    /// <inheritdoc cref="IAuditableEntity.LastUpdatedAt"/>
    /// </summary>
    public DateTime? LastUpdatedAt
    {
        get => _lastUpdatedAt;
        set => _lastUpdatedAt = value;
    }

    /// <summary>
    /// <inheritdoc cref="IAuditableEntity.LastUpdatedBy"/>
    /// </summary>
    public long? LastUpdatedBy
    {
        get => _lastUpdatedBy;
        set => _lastUpdatedAt = value;
    }
    /// <summary>
    /// <inheritdoc cref="IEntity.Id"/>
    /// </summary>
    public long Id 
    { 
        get => _id; 
        set => _id = value; 
    }

    /// <summary>
    /// <inheritdoc cref="IEntity.RowVersion"/>
    /// </summary>
    public string RowVersion 
    {
        get => _rowVersion;
        private set
        {
            _rowVersion = value;
        } 
    }

    /// <summary>
    /// <inheritdoc cref="ISoftDeleted.SoftDeleted"/>
    /// </summary>
    public bool SoftDeleted 
    { 
        get => _softDeleted; 
        set => _softDeleted = value; 
    }

    /// <summary>
    /// <inheritdoc cref="IEntity.DomainEvents"/>
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// returns true if current entity and the provided other are equal
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(object? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != GetType()) return false;
        return Equals(other);
    }

    /// <summary>
    /// Equaility comparison operator. returns true if both elements are equal
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==(Entity? a, Entity? b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// Dis-Equaility comparison operator 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=(Entity? a, Entity? b)
    {
        return !Equals(a, b);
    }

    /// <summary>
    /// <inheritdoc cref="object.GetHashCode"/>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
        return (GetType().GetHashCode() * 907) + (Id != null ? Id.GetHashCode() : 314);
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    /// <summary>
    /// <inheritdoc cref="object.ToString"/>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}, RowVersion={RowVersion}]";
    }

    /// <summary>
    /// <inheritdoc cref="IEquatable{T}"/>
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IEntity? other)
    {
        var compareTo = other as IEntity;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
        return Id != null;
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
    }
    public bool Equals(Entity other)
    {
        return _id!.Equals(other._id) && _softDeleted == other._softDeleted && _rowVersion == other._rowVersion;
    }

    /// <summary>
    /// <inheritdoc cref="IEntity.AddDomainEvent(IDomainEvent)"/>
    /// </summary>
    /// <param name="domainEvent"></param>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (!_domainEvents.Contains(domainEvent))
            _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// <inheritdoc cref="IEntity.RemoveDomainEvent(IDomainEvent)"/>
    /// </summary>
    /// <param name="domainEvent"></param>
    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        if(_domainEvents.Contains(domainEvent))
            _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// <inheritdoc cref="IEntity.ClearEvents"/>
    /// </summary>
    public void ClearEvents()
    {
        _domainEvents.Clear();
    }
}
