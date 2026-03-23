using System;
using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class RoleRepository(AppDbContext context) : Repository<Role>(context), IRoleRepository
{

}
