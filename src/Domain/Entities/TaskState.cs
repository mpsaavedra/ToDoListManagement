using Bootler.Domain.Entities.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Entities;

public abstract class TaskState
{
    protected Task Task { get; private set; }

    protected TaskState(Task task)
    {
        Task = task;
    }

    public static TaskState New(string stateType, Task task)
    {
        return stateType switch
        {
            nameof(TaskConfirmed) => new TaskConfirmed(task),
            nameof(TaskPending) => new TaskPending(task),
            nameof(TaskFinished) => new TaskFinished(task),
            _ => new TaskCreated(task)
        };
    }

    protected void Become(TaskState next)
    {
        next.Task = Task;
        Task.State = next;
        Task.StateType = next.GetType().Name;
    }
    protected internal virtual void Confirmed() => throw new InvalidOperationException();
    protected internal virtual void Pending() => throw new InvalidOperationException();
    protected internal virtual void Finished() => throw new InvalidOperationException();
}
