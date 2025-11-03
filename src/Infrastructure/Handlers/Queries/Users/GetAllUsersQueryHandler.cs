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

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, BaseResponse<GetAllUsersResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task<BaseResponse<GetAllUsersResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repo = _unitOfWork.Repository<User>();
            var data = await repo!.GetAllAsync(cancellationToken);

            if(!request.Input.SoftDeleted)
            {
                data = data.Where(x => !x.SoftDeleted);
            }

            // apply pagination
            var paginated = await data
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Input.PageIndex, request.Input.PageSize);

            return new BaseResponse<GetAllUsersResponse>
            {
                Data = new GetAllUsersResponse(paginated)
            };
        }
        catch (Exception ex)
        {
            Log.Error($"An error occurs while retrieving Users: {ex.Message}", ex);
            return BaseResponse.Fail<BaseResponse<GetAllUsersResponse>>($"An error occurs while retrieving Users: {ex.Message}");
        }
    }
}
