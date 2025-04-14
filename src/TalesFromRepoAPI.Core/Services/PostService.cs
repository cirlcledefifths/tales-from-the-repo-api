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

        public async Task<Post> GetPostByIdAsync(Guid id, string title)
        {
            return await _postRepository.GetByIdAsync(id, title);
        }

        // public async Task<Post> GetPostBySlugAsync(string slug)
        // {
        //     return await _postRepository.GetBySlugAsync(slug);
        // }

        // public async Task<List<Post>> GetPostsByAuthorAsync(Guid authorId)
        // {
        //     return await _postRepository.GetByAuthorAsync(authorId);
        // }

        // public async Task<List<Post>> GetPostsByTagAsync(string tag)
        // {
        //     return await _postRepository.GetByTagAsync(tag);
        // }

        // public async Task<Post> CreatePostAsync(Post post)
        // {
        //     return await _postRepository.CreateAsync(post);
        // }
        // public async Task<Post> UpdatePostAsync(Post post)
        // {
        //     return await _postRepository.UpdateAsync(post);
        // }
        // public async Task<bool> DeletePostAsync(Guid id)
        // {
        //     return await _postRepository.DeleteAsync(id);
        // }
        // public async Task<List<Post>> GetPostsByAuthorAndTagAsync(Guid authorId, string tag)
        // {
        //     var posts = await _postRepository.GetByAuthorAsync(authorId);
        //     return posts.FindAll(p => p.Tags.Contains(tag));
        // }
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
        // public async Task<List<Post>> GetPostsByAuthorAndDateRangeAsync(Guid authorId, DateTime startDate, DateTime endDate)
        // {
        //     var posts = await _postRepository.GetByAuthorAsync(authorId);
        //     return posts.FindAll(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);
        // }
    }
}
