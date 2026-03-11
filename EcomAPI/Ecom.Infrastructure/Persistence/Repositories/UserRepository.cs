using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
    {
        /// <summary>
        /// Async method to authenticate a user based on their account(email or phone) and password. 
        /// This method will check the provided credentials against the stored user data and return the corresponding User entity if the authentication is successful. 
        /// If the credentials are invalid, it will return null.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<User?> LoginAsync(string account, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => (u.Email == account || u.PhoneNumber == account) && u.IsDeleted == false);
            if (user == null)
                return null;
            var isCorrectPassword = BCrypt.Net.BCrypt.Verify(password, user?.Password);
            if (isCorrectPassword)
                return user;
            return null;
        }
    }
}
