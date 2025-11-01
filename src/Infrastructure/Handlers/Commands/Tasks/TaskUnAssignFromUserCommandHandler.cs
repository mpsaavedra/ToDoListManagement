using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Domain.Entities;
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

public class TaskUnAssignFromUserCommandHandler : IRequestHandler<TaskUnAssignFromUserCommand, BaseResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskUnAssignFromUserCommandHandler(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public Task<BaseResponse> Handle(TaskUnAssignFromUserCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ExecuteAsync(async () =>
        {
            try
            {
                var taskRepo = _unitOfWork.Repository<Domain.Entities.Task>();
                var userRepo = _unitOfWork.Repository<User>();
                var userTaskRepo = _unitOfWork.Repository<UserTask>();
                var task = await taskRepo!.FirstOrDefaultAsync(x => x.Id == request.TaskId,
                    cancellationToken: cancellationToken);
                if (task is null)
                {
                    Log.Error("Task {TaskId} not found", request.TaskId);
                    return BaseResponse.Failed("Task not found.");
                }
                var user = await userRepo!.FirstOrDefaultAsync(x => x.Id == request.UserId,
                    cancellationToken: cancellationToken);
                if (user is null)
                {
                    Log.Error("User {UserId} not found", request.UserId);
                    return BaseResponse.Failed("User not found.");
                }
                if (!user.Tasks.Any(x => x.UserId == request.UserId && x.TaskId == request.TaskId))
                {
                    Log.Debug("Task {TaskId} not assigned to User {UserId}", request.TaskId, request.UserId);
                    return BaseResponse.Failed($"Task {request.TaskId} not assigned to User {request.UserId}");
                }
                var userTask = await userTaskRepo!.FirstOrDefaultAsync(x => x.TaskId == task.Id &&  x.UserId == user.Id);
                task.UserTasks.Remove(userTask!);
                if(userTask == null)
                {
                    Log.Error("User {UserId} has no Task {TaskId} assigned", request.UserId, request.TaskId);
                    return BaseResponse.Failed($"User {request.UserId} has no Task {request.TaskId} assigned.");
                }
                await taskRepo.UpdateAsync(task.Id, task, cancellationToken: cancellationToken);
                return BaseResponse.Succeed($"Task {request.UserId} UnAssigned sucessfully");
            }
            catch (Exception ex)
            {
                Log.Error("Error while unassigning task", ex);
                return BaseResponse.Failed("Error while UnAssigning task");
            }

        });
}