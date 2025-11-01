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
using AppTask = Bootler.Domain.Entities.Task;

namespace Bootler.Infrastructure.Handlers.Commands.Tasks;

public class TaskAssignToUserCommandHandler : IRequestHandler<TaskAssignToUserCommand, BaseResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskAssignToUserCommandHandler(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse> Handle(TaskAssignToUserCommand request, CancellationToken cancellationToken) =>
        await _unitOfWork.ExecuteAsync(async () =>
        {
            try
            {
                Log.Debug("Assigning Task {TaskId} to User {UserId}", request.Input.TaskId, request.Input.UserId);

                var taskRepo = _unitOfWork.Repository<AppTask>();
                var userRepo = _unitOfWork.Repository<User>();
                var task = await taskRepo!.FirstOrDefaultAsync(x => x.Id == request.Input.TaskId,
                    cancellationToken: cancellationToken);
                if (task is null)
                {
                    Log.Error("Task {TaskId} not found", request.Input.TaskId);
                    return BaseResponse.Failed("Task not found.");
                }
                var user = await userRepo!.FirstOrDefaultAsync(x => x.Id == request.Input.UserId,
                    cancellationToken: cancellationToken);
                if (user is null)
                {
                    Log.Error("User {UserId} not found", request.Input.UserId);
                    return BaseResponse.Failed("User not found.");
                }
                if (!user.Tasks.Any(x => x.UserId == request.Input.UserId && x.TaskId == request.Input.TaskId))
                {
                    await taskRepo!.UpdateAsync(task.Id, task);
                    Log.Debug("Task {TaskId} assigned to User {UserId}", request.Input.TaskId, request.Input.UserId);
                }

                return BaseResponse.Succeed($"Task {request.Input.TaskId} assigned to user {request.Input.UserId} successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error assigning Task {TaskId} to User {UserId}", request.Input.TaskId, request.Input.UserId);
                return BaseResponse.Failed("An error occurred while assigning the task to the user.");
            }
        });
}
