using System;

namespace Application.Dtos.UserDtos;

public class UserRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public RoleDto Role { get; set; }
}
