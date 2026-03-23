using System;
using Application.Dtos;
using Application.Interfaces;
using Application.Interfaces.Services;

namespace Infrastructure.Services;

public class UserRoleService(IUnitOfWork unitOfWork) : IUserRoleService
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    public async Task<GenericResponse<bool>> UpdateUserRolesAsync(Guid userId, List<Guid> roleIds, Guid updatedBy)
    {
        try
        {
            var userRoleRepository = unitOfWork.UserRoleRepository;

            // Remove existing roles
            var existingRoles = await userRoleRepository.GetUserRolesByUserIdAsync(userId);

            foreach (var userRole in existingRoles)
            {
                if (!roleIds.Contains(userRole.RoleId))
                {
                    userRole.UpdatedBy = updatedBy;
                    await userRoleRepository.SoftDeleteAsync(userRole);
                }
            }

            // Add new roles
            foreach (var roleId in roleIds)
            {
                if (existingRoles.Any(ur => ur.RoleId == roleId))
                    continue;

                var userRole = new Domain.Entities.UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };
                userRole.CreatedBy = updatedBy;
                await userRoleRepository.AddAsync(userRole);
            }

            await unitOfWork.SaveChangesAsync();

            return new GenericResponse<bool>
            {
                Success = true,
                Message = "User roles updated successfully.",
                Data = true
            };

        }
        catch (Exception ex)
        {
            return new GenericResponse<bool>
            {
                Success = false,
                Message = $"An error occurred while updating user roles: {ex.Message}",
                Data = false
            };
        }
    }
}
