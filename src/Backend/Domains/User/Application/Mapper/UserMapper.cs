using Backend.Domains.User.Domain.DTO;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;
using Riok.Mapperly.Abstractions;

namespace Backend.Domains.User.Application.Mapper;

[Mapper]
public partial class UserMapper
{
    [MapperIgnoreSource(nameof(UserEntity.Hash))]
    [MapperIgnoreSource(nameof(UserEntity.IsInitialUser))]
    [MapperIgnoreSource(nameof(UserEntity.FullName))]
    public partial UserGetDto ToDto(UserEntity entity);

    public partial IEnumerable<UserGetDto> ToDto(IEnumerable<UserEntity> entities);

    private static Guid IdToGuid(UserId id)
    {
        return id.Value;
    }

    private static string? PhoneToString(Phone? phone)
    {
        return phone?.Value;
    }
}
