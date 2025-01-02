using Backend.Domains.Project.Domain.DTO;
using Backend.Domains.Project.Domain.Entities;
using Backend.Domains.User.Domain.DTO;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;
using Riok.Mapperly.Abstractions;

namespace Backend.Domains.Common.Application.Mapper;

[Mapper]
public partial class SsoMapper
{
    [MapperIgnoreSource(nameof(UserEntity.Hash))]
    [MapperIgnoreSource(nameof(UserEntity.IsInitialUser))]
    [MapperIgnoreSource(nameof(UserEntity.FullName))]
    [MapperIgnoreSource(nameof(UserEntity.Projects))]
    public partial UserGetDto ToDto(UserEntity entity);

    public partial IEnumerable<UserGetDto> ToDto(IEnumerable<UserEntity> entities);

    public partial ProjectGetDto ToDto(ProjectEntity entity);
    public partial IEnumerable<ProjectGetDto> ToDto(IEnumerable<ProjectEntity> entities);

    private static Guid UserIdToGuid(UserId id)
    {
        return id.Value;
    }

    private static string? PhoneToString(Phone? phone)
    {
        return phone?.Value;
    }
}
