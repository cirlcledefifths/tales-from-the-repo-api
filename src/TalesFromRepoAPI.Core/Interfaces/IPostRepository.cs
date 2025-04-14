using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalesFromRepoAPI.Core.Models;

namespace TalesFromRepoAPI.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync(bool includeUnpublished = false);
        Task<Post> GetByIdAsync(Guid id, string title);
        // Task<Post> GetBySlugAsync(string slug);
        // Task<List<Post>> GetByAuthorAsync(Guid authorId);
        // Task<List<Post>> GetByTagAsync(string tag);
        // Task<Post> CreateAsync(Post post);
        // Task<Post> UpdateAsync(Post post);
        // Task<bool> DeleteAsync(Guid id);
    }
}