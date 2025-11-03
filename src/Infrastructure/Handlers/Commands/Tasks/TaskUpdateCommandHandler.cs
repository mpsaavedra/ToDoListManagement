using AutoMapper;
using Bootler.Contracts.Responses;
using Bootler.Infrastructure.Commands.Tasks;
using Bootler.Infrastructure.Common;
using Bootler.Infrastructure.Extensions;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Handlers.Commands.Tasks;

public class TaskUpdateCommandHandler : IRequestHandler<TaskUpdateCommand, BaseResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskUpdateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public Task<BaseResponse> Handle(TaskUpdateCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ExecuteAsync(async () =>
        {
            try
            {
                var data = _mapper.Map<Domain.Entities.Task>(request.Input);
                var taskRepo = _unitOfWork.Repository<Domain.Entities.Task>();
                var task = await taskRepo!.FirstOrDefaultAsync(x => x.Id == request.Input.Id);
                if (task is null)
                {
                    Log.Error("Task {TaskId} not found", request.Input.Id);
                    return BaseResponse.Failed("Task not found.");
                }

                //task.Title = request.Input.Title!;
                //task.Description = request.Input.Description!;
                //task.DueDate = request.Input.DueDate;
                //task.StateType = request.Input.StateType!;
                task.PopulateWithMappedData(data);
                await taskRepo.UpdateAsync(task.Id, task);

                Log.Debug($"Task {request.Input.Id} updated successfully");

                return BaseResponse.Succeed();
            }
            catch (Exception ex)
            {
                Log.Error("An error has occurs while updating task", ex);
                return BaseResponse.Failed($"An error ocurrs while updating task {request.Input.Id}");
            }
        });
}
