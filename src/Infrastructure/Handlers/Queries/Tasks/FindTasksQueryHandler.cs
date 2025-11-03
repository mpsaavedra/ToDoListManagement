using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bootler.Contracts.DTOs.Users;
using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Tasks;
using Bootler.Contracts.Responses.Users;
using Bootler.Domain.Entities;
using Bootler.Infrastructure.Common;
using Bootler.Infrastructure.Queries.Tasks;
using Bootler.Infrastructure.Extensions;
using AppTask = Bootler.Domain.Entities.Task;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bootler.Contracts.DTOs.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bootler.Infrastructure.Handlers.Queries.Tasks;

public class FindTasksQueryHandler : IRequestHandler<FindTasksQuery, BaseResponse<FindTasksResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FindTasksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task<BaseResponse<FindTasksResponse>> Handle(FindTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _unitOfWork.Repository<AppTask>();
            var data = await repo.FindAsync(include:
                i => i
                    .Include(x => x.UserTasks)
                    .ThenInclude(x => x.User),
                cancellationToken: cancellationToken);
            // apply order by if exists
            if (!string.IsNullOrEmpty(request.Input.OrderBy) && !request.Input.OrderBy!.IsNullEmptyOrWhiteSpace())
                data = data.OrderBy(request.Input.OrderBy!);

            // apply filters if exists
            if (request.Input.Filters != null && request.Input.Filters.Any())
            {
                foreach (var filter in request.Input.Filters)
                {
                    data = data.Where(filter);
                }
            }
            var d = true;
            // apply pagination
            var projectTo = data.ProjectTo<TaskDetailDto>(_mapper.ConfigurationProvider);
            var paginated = await projectTo.PaginatedListAsync(request.Input.PageIndex, request.Input.PageSize);

            return new BaseResponse<FindTasksResponse>
            {
                Data = new FindTasksResponse(paginated)
            };
        }
        catch (Exception ex)
        {
            Log.Error("An error occurs while retrieving Tasks", ex);
            return BaseResponse.Fail<BaseResponse<FindTasksResponse>>(ex);
        }
    }
}
