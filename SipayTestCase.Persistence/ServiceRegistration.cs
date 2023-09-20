using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SipayTestCase.Application.Interfaces;
using SipayTestCase.Persistence.Context;
using SipayTestCase.Persistence.Repositories;

namespace SipayTestCase.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DbConnection"));
        });
    }
}