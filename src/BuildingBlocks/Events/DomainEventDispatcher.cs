using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Events;

/// <summary>
/// Domain event dispatcher
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// dispatch a list of events using the mediator bus
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    Task DispatchEventAsync(IEnumerable<IDomainEvent> events);
}

/// <summary>
/// <inheritdoc cref="IDomainEventDispatcher"/>
/// </summary>
public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    /// <summary>
    /// <inheritdoc cref="IDomainEventDispatcher.DispatchEventAsync(IEnumerable{IDomainEvent})"/>
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    public async Task DispatchEventAsync(IEnumerable<IDomainEvent> events)
    {
        foreach(var @event in events)
        {
            await mediator.Publish(@event);
        }
    }
}
