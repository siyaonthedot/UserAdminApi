using System;
using Domain.Entities;

namespace Application.Interfaces.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersWithRolesAsync(int pageNumber, int pageSize);
}
