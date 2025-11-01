using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bootler.Contracts.DTOs.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Tasks;
using Bootler.Infrastructure.Common;
using Bootler.Infrastructure.Queries.Tasks;
using AppTask = Bootler.Domain.Entities.Task;
using MediatR;
using Serilog;
using Bootler.Infrastructure.Extensions;

namespace Bootler.Infrastructure.Handlers.Queries.Tasks;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, BaseResponse<GetAllTasksResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTasksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }
    public async Task<BaseResponse<GetAllTasksResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
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
            return (BaseResponse<GetAllTasksResponse>)BaseResponse.Failed("An error occurs while retrieving Taks");
        }
    }
}