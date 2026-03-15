using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Abstractions.Persistence
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        //Task<bool> ExistsByEmailAsync(string email);
        //Task<bool> ExistsByUserNameAsync(string username);
        Task<User?> LoginAsync(string username, string password);
        Task<User?> GetByEmailAsync(string email);
        //Task<User?> GetByUserIdAsync(Guid userId);
        //Task<User> GetByUserNameAsync(string userName);
        //Task<IEnumerable<User>> GetAllUsersWithRoleUserAsync();
        //Task<bool> isActivePremium(Guid userId);
    }
}
