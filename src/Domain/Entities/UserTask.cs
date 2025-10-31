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
    public long _taskId;
    private Task _task;
    private long _userId;
    private User _user;
    public long UserId 
    { 
        get => _userId; 
        set => _userId = value; 
    }
    public User User { 
        get => _user; 
        set => _user = value; 
    }
    public long TaskId 
    { 
        get => _taskId;
        set => _taskId = value; 
    }
    public Task Task 
    { 
        get => _task;
        set => _task = value; 
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
