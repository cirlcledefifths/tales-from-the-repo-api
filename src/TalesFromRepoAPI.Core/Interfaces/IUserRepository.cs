using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalesFromRepoAPI.Core.Models;

namespace TalesFromRepoAPI.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        // Task<User> GetByIdAsync(Guid id);
        // Task<User> GetByUsernameAsync(string username);
        // Task<User> CreateAsync(User user);
        // Task<User> UpdateAsync(User user);
        // Task<bool> DeleteAsync(Guid id);
    }
}