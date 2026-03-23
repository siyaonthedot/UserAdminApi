using System;
using Domain.Entities;

namespace Application.Dtos.UserDtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Oid { get; set; }
    public List<UserRoleDto> UserRoles { get; set; } = [];
}
