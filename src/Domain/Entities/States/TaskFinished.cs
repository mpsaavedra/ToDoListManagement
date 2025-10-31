using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Domain.Entities.States;

public class TaskFinished : TaskState
{
    public TaskFinished(Task task) : base(task)
    {
    }
}
