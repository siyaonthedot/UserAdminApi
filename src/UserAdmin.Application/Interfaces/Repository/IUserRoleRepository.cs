using System;
using Domain.Entities;

namespace Application.Interfaces.Repository;

public interface IUserRoleRepository : IRepository<UserRole>
{
    Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(Guid userId);
}
