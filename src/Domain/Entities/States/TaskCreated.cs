using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Entities.States;

public class TaskCreated : TaskState
{
    public TaskCreated(Task task) : base(task)
    {
    }

    protected internal override void Pending()
    {
        Become(new TaskConfirmed(Task));
    }
}
