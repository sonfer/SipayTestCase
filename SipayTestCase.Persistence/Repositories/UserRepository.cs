using SipayTestCase.Application.Interfaces;
using SipayTestCase.Domain.Entities;
using SipayTestCase.Persistence.Context;

namespace SipayTestCase.Persistence.Repositories;

public class UserRepository:GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}