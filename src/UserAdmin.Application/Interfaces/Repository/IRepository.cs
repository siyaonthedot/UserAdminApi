using System;
using Domain.Entities;

namespace Application.Interfaces.Repository;

public interface IRepository<T> where T : BaseEntity
{
    Task <T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task SoftDeleteAsync(T entity);
}
