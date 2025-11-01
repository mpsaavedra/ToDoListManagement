using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bootler.Contracts.DTOs.Users;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Users;
using Bootler.Domain.Entities;
using Bootler.Infrastructure.Common;
using Bootler.Infrastructure.Extensions;
using Bootler.Infrastructure.Queries.Users;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Handlers.Queries.Users;

public class FindUsersQueryHandler : IRequestHandler<FindUsersQuery, BaseResponse<FindUsersResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FindUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task<BaseResponse<FindUsersResponse>> Handle(FindUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _unitOfWork.Repository<User>();
            var data = await repo.GetAllAsync(cancellationToken);
            
            // apply order by if exists
            if (!request.Input.OrderBy!.IsNullEmptyOrWhiteSpace())
                data = data.OrderBy(request.Input.OrderBy!);
            
            // apply filters if exists
            if(request.Input.Filters.Any())
            {
                foreach (var filter in request.Input.Filters)
                {
                    data = data.Where(filter);
                }
            }

            // apply pagination
            var paginated = await data
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Input.PageIndex, request.Input.PageSize);

            return new BaseResponse<FindUsersResponse>
            {
                Data = new FindUsersResponse(paginated)
            };
        }
        catch (Exception ex) 
        {
            Log.Error("An error occurs while Searching for Users", ex);
            return (BaseResponse<FindUsersResponse>)BaseResponse.Failed("An error occurs while searching for Users");
        }
    }
}
