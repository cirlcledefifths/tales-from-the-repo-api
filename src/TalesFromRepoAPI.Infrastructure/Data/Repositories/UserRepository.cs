using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using TalesFromRepoAPI.Core.Interfaces;
using TalesFromRepoAPI.Core.Models;
using TalesFromRepoAPI.Infrastructure.Data.Entities;

namespace TalesFromRepoAPI.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly DynamoDBContext _context;

        public UserRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
            _context = new DynamoDBContext(_dynamoDb);
        }

        public async Task<List<User>> GetAllAsync()
        {
            var search = _context.ScanAsync<UserEntity>(new List<ScanCondition>());
            var users = await search.GetRemainingAsync();
            return users.Select(u => new User
            {
                Id = Guid.Parse(u.Id),
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                Posts = u.Posts.Select(p => new Post
                {
                    Id = Guid.Parse(p)
                }).ToList()
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

        // public async Task<User> CreateAsync(User user)
        // {
        //     var userEntity = new UserEntity
        //     {
        //         Id = user.Id.ToString(),
        //         Username = user.Username,
        //         Email = user.Email,
        //         CreatedAt = DateTime.UtcNow
        //     };

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
    }
}