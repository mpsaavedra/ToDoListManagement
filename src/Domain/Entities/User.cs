using Bootler.Data;
using Bootler.Domain.Entities.States;
using Bootler.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Entities;

public class User : Entity
{
    private string _userName;
    private string _password;
    private string? _token = null;
    private long _roleId;
    private Role _role;
    private ICollection<UserTask> _tasks = new List<UserTask>();
    private ICollection<UserTask> _assignedTasks = new List<UserTask>();

    protected User() { }
    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
        AddDomainEvent(new UserCreated(userName));
    }

    public static User Create(string userName, string Password) =>
        new (userName, Password);

    public string UserName 
    { 
        get => _userName; 
        set => _userName = value; 
    }    
    public string Password 
    { 
        get => _password; 
        set => _password = value; 
    }
    public string? Token 
    { 
        get => _token; 
        set => _token = value; 
    }
    public ICollection<UserTask> Tasks 
    { 
        get => _tasks; 
        set => _tasks = value;
    }
    public ICollection<UserTask> AssignedTasks
    {
        get => _assignedTasks;
        set => _assignedTasks = value;
    }

    public long RoleId
    {
        get => _roleId;
        set => _roleId = value;
    }

    public Role Role
    {
        get => _role;
        set => _role = value;
    }
}
