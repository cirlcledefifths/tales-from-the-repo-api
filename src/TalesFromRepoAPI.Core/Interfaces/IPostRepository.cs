using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalesFromRepoAPI.Core.Models;

namespace TalesFromRepoAPI.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync(bool includeUnpublished = false);
        Task<Post> GetByIdAsync(Guid id);
        Task<Post> CreatePostAsync(Post post);
        // Task<List<Post>> GetByTagAsync(string tag);
        Task<Post> UpdateAsync(Post post);
        Task<bool> DeleteAsync(Guid id);
    }
}