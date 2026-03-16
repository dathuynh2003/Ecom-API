using Ecom.Application.Abstractions.Persistence;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
    {
        public async Task<bool> IsPhoneInUseAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber) is not null;
        }

        /// <summary>
        /// Determines whether a user with the specified phone number exists in the database, excluding the user with
        /// the given email address.
        /// </summary>
        /// <param name="email">The email address to exclude from the search. Typically used to prevent matching the current user's record.</param>
        /// <param name="phoneNumber">The phone number to check for existence among users other than the one with the specified email address.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if a user with
        /// the specified phone number exists and has a different email address; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> IsPhoneInUseByAnotherUserAsync(string email, string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Email != email) is not null;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }

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
