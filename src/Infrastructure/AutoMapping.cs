using AutoMapper;
using Bootler.Contracts.DTOs.Roles;
using Bootler.Contracts.DTOs.Tasks;
using Bootler.Contracts.DTOs.Users;
using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Requests.Users;
using Bootler.Domain.Entities;

namespace Bootler.Infrastructure;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        // users mappings
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserDetailDto>().ReverseMap();
        CreateMap<UserTask, UserTaskDto>().ReverseMap();
        CreateMap<User, SignUpRequest>().ReverseMap();

        // tasks mappings
        CreateMap<Domain.Entities.Task, TaskDto>().ReverseMap();
        CreateMap<Domain.Entities.Task, TaskDetailDto>().ReverseMap();
        CreateMap<TaskCreateRequest, Domain.Entities.Task>();
        CreateMap<TaskUpdateRequest, Domain.Entities.Task>();

        // role mappings
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleSingleDto>().ReverseMap();
    }
}
