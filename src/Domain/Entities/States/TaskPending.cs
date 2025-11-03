using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Entities.States;

public class TaskPending : TaskState
{
    public TaskPending(Task task) : base(task)
    {
    }

    protected internal override void Finished()
    {
        Become(new TaskFinished(Task));
    }
}
