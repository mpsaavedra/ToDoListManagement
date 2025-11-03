using Bootler.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Events;

/// <summary>
/// notify that a new user have been created
/// </summary>
/// <param name="Username"></param>
public class UserCreated(string Username) : IDomainEvent;
