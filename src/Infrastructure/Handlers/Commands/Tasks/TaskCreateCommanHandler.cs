using AutoMapper;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Tasks;
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

public class TaskCreateCommanHandler : IRequestHandler<TaskCreateCommand, BaseResponse<TaskCreateResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskCreateCommanHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }
    public Task<BaseResponse<TaskCreateResponse>> Handle(TaskCreateCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ExecuteAsync(async () =>
        {
            try
            {
                Log.Debug($"Creating new Task");
                var taskRepo = _unitOfWork.Repository<Domain.Entities.Task>();
                var task = _mapper.Map<Domain.Entities.Task>(request.Input);
                task.StateType = "TaskCreated";
                var newTask = await taskRepo!.CreateAsync(task, cancellationToken);
                var id = newTask.HasValue ? newTask.Value : 0;
                Log.Debug($"Task created successfully");
                return new BaseResponse<TaskCreateResponse>
                {
                    Data = new TaskCreateResponse(id)
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating Task: {ex.Message}");
                return BaseResponse.Fail<BaseResponse<TaskCreateResponse>>("Error creating Task");
            }
        });
}
