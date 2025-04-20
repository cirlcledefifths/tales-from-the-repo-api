using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalesFromRepoAPI.Core.Interfaces;
using TalesFromRepoAPI.Core.Models;

namespace TalesFromRepoAPI.Core.Services
{
    public class PostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<Post>> GetAllPostsAsync(bool includeUnpublished = false)
        {
            return await _postRepository.GetAllAsync(includeUnpublished);
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            return await _postRepository.GetByIdAsync(id);
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            return await _postRepository.CreatePostAsync(post);
        }

        // public async Task<List<Post>> GetPostsByTagAsync(string tag)
        // {
        //     return await _postRepository.GetByTagAsync(tag);
        // }

        public async Task<Post> UpdatePostAsync(Post post)
        {
            return await _postRepository.UpdateAsync(post);
        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            return await _postRepository.DeleteAsync(id);
        }
        
        // public async Task<List<Post>> GetPostsByTagAndDateRangeAsync(string tag, DateTime startDate, DateTime endDate)
        // {
        //     var posts = await _postRepository.GetByTagAsync(tag);
        //     return posts.FindAll(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);
        // }
        // public async Task<List<Post>> GetPostsByDateRangeAsync(DateTime startDate, DateTime endDate)
        // {
        //     var posts = await _postRepository.GetAllAsync();
        //     return posts.FindAll(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);
        // }
    }
}
