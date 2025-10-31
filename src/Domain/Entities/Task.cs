using Bootler.Data;
using Bootler.Domain.Entities.States;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Entities;

public partial class Task : Entity
{
    private TaskState _todoState; 
    private string _title;
    private string _description;
    private string _stateType;
    private DateTime? _dueDate;
    private ICollection<UserTask> _userTasks = new List<UserTask>();

    protected Task() { }

    public Task(string title, string description, string stateType, DateTime? dueDate = null)
    {
        _title = title;
        _description = description;
        _stateType = stateType;
        _dueDate = dueDate;
    }

    public static Task Create(string title, string description, string stateType, DateTime? dueDate = null) =>
        new(title, description, stateType, dueDate);

    public string Title 
    { 
        get => _title; 
        private set => _title = value; 
    }
    public string Description 
    { 
        get => _description; 
        private set => _description = value; 
    }
    public string StateType 
    { 
        get => _stateType; 
        set => _stateType = value; 
    }
    public DateTime? DueDate 
    { 
        get => _dueDate; 
        set => _dueDate = value; 
    }

    public ICollection<UserTask> UserTasks
    {
        get => _userTasks;
        set => _userTasks = value;
    }

    [NotMapped]
    public bool IsCompleted =>
        _todoState != null && _todoState.GetType() == typeof(TaskFinished);

    [NotMapped]
    public TaskState State
    {
        get => _todoState ??= TaskState.New(StateType, this);
        set => _todoState = value;
    }

    public override string ToString()
    {
        return $"Id:{Id}, Title:{Title}, State:{StateType}";
    }
}
