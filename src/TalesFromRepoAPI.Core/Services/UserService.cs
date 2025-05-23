using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalesFromRepoAPI.Core.Interfaces;
using TalesFromRepoAPI.Core.Models;

namespace TalesFromRepoAPI.Core.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        // public async Task<User> GetByIdAsync(Guid id)
        // {
        //     return await _userRepository.GetByIdAsync(id);
        // }

        // public async Task<User> GetByUsernameAsync(string username)
        // {
        //     return await _userRepository.GetByUsernameAsync(username);
        // }

        public async Task<User> CreateAsync(User user)
        {
            return await _userRepository.CreateAsync(user);
        }

        // public async Task<User> UpdateAsync(User user)
        // {
        //     return await _userRepository.UpdateAsync(user);
        // }

        // public async Task<bool> DeleteAsync(Guid id)
        // {
        //     return await _userRepository.DeleteAsync(id);
        // }
    }
}