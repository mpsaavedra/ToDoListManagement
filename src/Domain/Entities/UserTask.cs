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
    private long? _assignedById;
    private User? _assignedBy;
    public long _taskId;
    private Task _task;
    private long _userId;
    private User _user;

    public UserTask()
    {
    }
    public UserTask(long userId, long taskId, long? assignedById = null)
    {
        UserId = userId;
        TaskId = taskId;
        _assignedById = assignedById;
    }

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
    public long? AssignedById
    {
        get => _assignedById;
        set => _assignedById = value;
    }
    public User? AsignedBy
    {
        get => _assignedBy;
        set => _assignedBy = value;
    }
}
