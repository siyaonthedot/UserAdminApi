using System;
using System.Diagnostics;
using Application.Dtos;
using Application.Dtos.UserDtos;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Validators;
using Mapster;

namespace Infrastructure.Services;

public class UserService(IUnitOfWork unitOfWork, RequestUserDtoValidator validator) : IUserService
{
    private readonly RequestUserDtoValidator validator = validator;

    public async Task<GenericResponse<Guid>> CreateUserAsync(RequestUserDto createUserDto)
    {
        try
        {
            var validate = validator.Validate(createUserDto);

            if (!validate.IsValid)
            {
                return new GenericResponse<Guid>
                {
                    Success = false,
                    Message = string.Join("; ", validate.Errors.Select(e => e.ErrorMessage))
                };
            }

            var user = createUserDto.Adapt<Domain.Entities.User>();
            //user.UpdatedBy = createUserDto.UpdatedBy;
            //user.CreatedBy = createUserDto.UpdatedBy;
            
            await unitOfWork.UserRepository.AddAsync(user);

            //foreach (var roleId in createUserDto.RoleIds)
            //{
            //    var userRole = new Domain.Entities.UserRole
            //    {
            //        UserId = user.Id,
            //        RoleId = roleId
            //    };

            //    await unitOfWork.UserRoleRepository.AddAsync(userRole);
            //}

            await unitOfWork.SaveChangesAsync();

            return new GenericResponse<Guid>
            {
                Success = true,
                Data = user.Id // Use the actual created user ID
            };

        }
        catch (Exception ex)
        {
            return new GenericResponse<Guid>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<GenericResponse<bool>> DeleteUserAsync(DeleteUserDto deleteUserDto)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(deleteUserDto.UserId);
            if (user == null)
            {
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            user.UpdatedBy = deleteUserDto.DeletedBy;
            await unitOfWork.UserRepository.SoftDeleteAsync(user);

            await unitOfWork.SaveChangesAsync();

            return new GenericResponse<bool>
            {
                Success = true,
                Data = true
            };

        }
        catch (Exception ex)
        {
            return new GenericResponse<bool>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<GenericResponse<List<UserDto>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
    {
        var users = await unitOfWork.UserRepository.GetUsersWithRolesAsync(pageNumber, pageSize);
        
        var userDtos = users.Adapt<List<UserDto>>();
        
        return new GenericResponse<List<UserDto>>
        {
            Success = true,
            Data = userDtos
        };
    }

    public async Task<GenericResponse<UserDto>> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return new GenericResponse<UserDto>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var userDto = user.Adapt<UserDto>();

            return new GenericResponse<UserDto>
            {
                Success = true,
                Data = userDto
            };  
            
        }catch (Exception ex)
        {
            return new GenericResponse<UserDto>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<GenericResponse<UserDto>> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                return new GenericResponse<UserDto>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var userDto = user.Adapt<UserDto>();

            return new GenericResponse<UserDto>
            {
                Success = true,
                Data = userDto
            };

        }
        catch (Exception ex)
        {
            return new GenericResponse<UserDto>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<GenericResponse<Guid>> UpdateUserAsync(RequestUserDto updateUserDto, Guid userId)
    {
        try
        {
            var validate = validator.Validate(updateUserDto);

            if (!validate.IsValid)
            {
                return new GenericResponse<Guid>
                {
                    Success = false,
                    Message = string.Join("; ", validate.Errors.Select(e => e.ErrorMessage))
                };
            }

            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GenericResponse<Guid>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Email = updateUserDto.Email;
            //user.OId = updateUserDto.Oid;
            //user.UpdatedBy = updateUserDto.UpdatedBy;

            await unitOfWork.UserRepository.UpdateAsync(user);

            // Update roles            

            await unitOfWork.SaveChangesAsync();

            return new GenericResponse<Guid>
            {
                Success = true,
                Data = user.Id
            };
            
        }catch (Exception ex)
        {
            return new GenericResponse<Guid>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
}
