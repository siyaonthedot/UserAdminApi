using System;
using Application.Dtos;
using Application.Dtos.UserDtos;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<GenericResponse<Guid>> CreateUserAsync(RequestUserDto createUserDto);
    Task<GenericResponse<Guid>> UpdateUserAsync(RequestUserDto updateUserDto, Guid userId);
    Task<GenericResponse<bool>> DeleteUserAsync(DeleteUserDto deleteUserDto);
    Task<GenericResponse<UserDto>> GetUserByEmailAsync(string email);
    Task<GenericResponse<UserDto>> GetUserByIdAsync(Guid id);
    Task<GenericResponse<List<UserDto>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);
}
