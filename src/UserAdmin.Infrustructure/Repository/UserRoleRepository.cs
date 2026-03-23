using System;
using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserRoleRepository(AppDbContext context) : Repository<UserRole>(context), IUserRoleRepository
{
    public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(Guid userId)
    {
        return await _dbSet.Where(ur => ur.UserId == userId && !ur.IsDeleted).ToListAsync();
    }
}
