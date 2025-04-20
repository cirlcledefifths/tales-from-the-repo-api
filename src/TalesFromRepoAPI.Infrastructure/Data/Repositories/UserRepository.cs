using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using TalesFromRepoAPI.Core.Interfaces;
using TalesFromRepoAPI.Core.Models;
using TalesFromRepoAPI.Infrastructure.Data.Entities;
using Microsoft.Extensions.Logging;

namespace TalesFromRepoAPI.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
       private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<PostRepository> _logger;

        public UserRepository(IDynamoDBContext dynamoDbContext, ILogger<PostRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext ?? throw new ArgumentNullException(nameof(dynamoDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<User>> GetAllAsync()
        {
            var search = _dynamoDbContext.ScanAsync<UserEntity>(new List<ScanCondition>());
            var users = await search.GetRemainingAsync();
            return users.Select(u => new User
            {
                Id = Guid.Parse(u.Id),
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();
        }

        // public async Task<User> GetByIdAsync(Guid id)
        // {
        //     var userEntity = await _context.LoadAsync<UserEntity>(id.ToString());
        //     if (userEntity == null) return null;

        //     return new User
        //     {
        //         Id = Guid.Parse(userEntity.Id),
        //         Username = userEntity.Username,
        //         Email = userEntity.Email,
        //         CreatedAt = userEntity.CreatedAt,
        //         Posts = userEntity.Posts.Select(p => Guid.Parse(p)).ToList()
        //     };
        // }

        // public async Task<User> GetByUsernameAsync(string username)
        // {
        //     var search = _context.QueryAsync<UserEntity>(username);
        //     var users = await search.GetRemainingAsync();
        //     var userEntity = users.FirstOrDefault();
        //     if (userEntity == null) return null;

        //     return new User
        //     {
        //         Id = Guid.Parse(userEntity.Id),
        //         Username = userEntity.Username,
        //         Email = userEntity.Email,
        //         CreatedAt = userEntity.CreatedAt,
        //         Posts = userEntity.Posts.Select(p => Guid.Parse(p)).ToList()
        //     };
        // }

        public async Task<User> CreateAsync(User user)
        {
            try {
                var userEntity = new UserEntity
                {
                    Id = user.Id.ToString(),
                    Username = user.Username,
                    Email = user.Email,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                await _dynamoDbContext.SaveAsync(userEntity);
                return MapToUser(userEntity);
            }
            catch(Exception ex) {
                // Handle exceptions (e.g., log them)
                Console.WriteLine($"Error creating user: {ex.Message}");
                return null;
            }
        }

        //     await _context.SaveAsync(userEntity);
        //     return user;
        // }

        // public async Task<User> UpdateAsync(User user)
        // {
        //     var userEntity = await _context.Load Async<UserEntity>(user.Id.ToString());
        //     if (userEntity == null) return null;

        //     userEntity.Username = user.Username;
        //     userEntity.Email = user.Email;

        //     await _context.SaveAsync(userEntity);
        //     return user;
        // }

        // public async Task<bool> DeleteAsync(Guid id)
        // {
        //     var userEntity = await _context.LoadAsync<UserEntity>(id.ToString());
        //     if (userEntity == null) return false;

        //     await _context.DeleteAsync(userEntity);
        //     return true;
        // }

        #region Private Helper Methods

        private User MapToUser(UserEntity entity)
        {
            if (entity == null) return null;
            _logger.LogInformation($"Mapping user entity {JsonSerializer.Serialize(entity)}");
            return new User
            {
                Id = Guid.Parse(entity.Id),
                Username = entity.Username,
                Email = entity.Email,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            };
        }

        #endregion Private Helper Methods
    }
}