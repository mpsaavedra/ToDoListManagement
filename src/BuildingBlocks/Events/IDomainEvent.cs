using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Events;

/// <summary>
/// base domain event
/// </summary>
public interface IDomainEvent : INotification
{
}
