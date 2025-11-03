using Bootler.Contracts.Responses;
using Bootler.Infrastructure.Commands.Tasks;
using Bootler.Infrastructure.Common;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Handlers.Commands.Tasks;

public class TaskDeleteCommandHandler : IRequestHandler<TaskDeleteCommand, BaseResponse>
{
    private readonly IUnitOfWork unitOfWork;

    public TaskDeleteCommandHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    public Task<BaseResponse> Handle(TaskDeleteCommand request, CancellationToken cancellationToken) =>
        unitOfWork.ExecuteAsync(async () =>
        {
            try
            {
                Log.Debug($"Deleting Task with ID: {request.Input.TaskId}");
                var taskRepo = unitOfWork.Repository<Domain.Entities.Task>();
                var task = await taskRepo!.FirstOrDefaultAsync(x => x.Id == request.Input.TaskId, cancellationToken: cancellationToken);
                if (task == null)
                {
                    return BaseResponse.Failed("Task not found");
                }
                await taskRepo.DeleteAsync(task.Id, softDelete: request.Input.SoftDelete, cancellationToken: cancellationToken);
                Log.Debug("Task deleted successfully");
                return BaseResponse.Succeed("Task deleted successfully");
            }
            catch (Exception ex)
            {
                Log.Error("Error deleting Task", ex);
                return BaseResponse.Failed("Error deleting Task");
            }
        });
}
