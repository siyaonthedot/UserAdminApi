using System;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Application.Validators;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repository;
using Infrastructure.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure services here
        // e.g., services.AddTransient<IMyService, MyService>();

        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

    //  Services.AddDbContext<AppDbContext>(options =>
    //options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

        services.AddValidatorsFromAssemblies(new[] 
        { 
            typeof(RequestUserDtoValidator).Assembly,
            typeof(DependencyInjection).Assembly
        });

        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddMapster();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        return services;
    }
}
