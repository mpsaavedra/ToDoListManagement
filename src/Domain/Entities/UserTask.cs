using Bootler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Entities;

/// <summary>
/// M2M relation User with Tasks
/// </summary>
public class UserTask : Entity
{
    private long? _asignedById;
    private User? _asignedBy;
    public long UserId 
    { 
        get; 
        set; 
    }
    public User User { 
        get; 
        set; 
    }
    public long TaskId 
    { 
        get;
        set; 
    }
    public Task Task 
    { 
        get;
        set; 
    }
    public long? AsignedById
    {
        get => _asignedById;
        set => _asignedById = value;
    }
    public User? AsignedBy
    {
        get => _asignedBy;
        set => _asignedBy = value;
    }
}
