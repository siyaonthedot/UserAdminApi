using System;
using Application.Dtos;

namespace Application.Interfaces.Services;

public interface IUserRoleService
{
    Task<GenericResponse<bool>> UpdateUserRolesAsync(Guid userId, List<Guid> roleIds, Guid updatedBy);
}
