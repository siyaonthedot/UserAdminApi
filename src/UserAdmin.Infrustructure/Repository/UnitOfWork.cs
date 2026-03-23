using System;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repository;

public class UnitOfWork(
IUserRepository userRepository,
AppDbContext context,
IRoleRepository roleRepository,
IUserRoleRepository userRoleRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IRoleRepository RoleRepository => roleRepository;
    public IUserRoleRepository UserRoleRepository => userRoleRepository;
    private readonly AppDbContext _context = context;
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if(_transaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            
        }catch(Exception)
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if(_transaction == null)
        {
            throw new InvalidOperationException("No active transaction to rollback.");
        }

        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
