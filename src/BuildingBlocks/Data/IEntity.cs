using Bootler.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Data;

/// <summary>
/// Basic business entity information
/// </summary>
public interface IEntity
{
    /// <summary>
    /// the id of the entity
    /// </summary>
    long Id { get; set; }

    /// <summary>
    /// to avoid any row data repetition or collission
    /// </summary>
    string RowVersion { get; }

    /// <summary>
    /// if true entity will not be return in common queries, only will
    /// be displayed in administrative queries
    /// </summary>
    bool SoftDeleted { get; set; }

    /// <summary>
    /// id of user that creates the entity
    /// </summary>
    long? CreatedBy { get; set; }
    /// <summary>
    /// Creation date
    /// </summary>
    DateTime CreatedAt { get; set; }
    /// <summary>
    /// Last update date time
    /// </summary>
    DateTime? LastUpdatedAt { get; set; }
    /// <summary>
    /// 
    /// </summary>
    long? LastUpdatedBy { get; set; }

    /// <summary>
    /// Returns a <see cref="IReadOnlyCollection{IDomainEvent}"/> collection
    /// with registered events
    /// </summary>
    [NotMapped]
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Add a new IDomainEvent
    /// </summary>
    /// <param name="domainEvent"></param>
    void AddDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Remove some specific IDomainEvent 
    /// </summary>
    /// <param name="domainEvent"></param>
    void RemoveDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// clear all Domain Events of this entity
    /// </summary>
    void ClearEvents();
}
