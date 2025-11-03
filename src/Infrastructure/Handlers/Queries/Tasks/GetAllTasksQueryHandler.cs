using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bootler;
using Bootler.Contracts.DTOs.Tasks;
using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Tasks;
using Bootler.Infrastructure.Common;
using Bootler.Infrastructure.Extensions;
using Bootler.Infrastructure.Queries.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using AppTask = Bootler.Domain.Entities.Task;

namespace Bootler.Infrastructure.Handlers.Queries.Tasks;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, BaseResponse<GetAllTasksResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _provider;

    public GetAllTasksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IServiceProvider provider)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
        this._provider = provider;
    }
    public async Task<BaseResponse<GetAllTasksResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            //var validator = _provider.GetRequiredService<IValidator<GetAllTasksRequest>>();
            //var validation = await validator.ValidateAsync(request.Input);
            //validation.IsInvalidThrow();

            var repo = _unitOfWork.Repository<AppTask>();
            var data = await repo.GetAllAsync(cancellationToken);

            if (!request.Input.SoftDeleted)
            {
                data = data.Where(x => !x.SoftDeleted);
            }

            // apply pagination
            var paginated = await data
                    .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Input.PageIndex, request.Input.PageSize);

            return new BaseResponse<GetAllTasksResponse>
            {
                Data = new GetAllTasksResponse(paginated)
            };
        }
        catch (Exception ex)
        {
            Log.Error("An error occurs while retrieving Tasks", ex);
            // Use the generic factory to produce the correct typed response
            return BaseResponse.Fail<BaseResponse<GetAllTasksResponse>>(ex);
        }
    }
}