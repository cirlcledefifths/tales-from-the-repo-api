using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalesFromRepoAPI.Core.Interfaces;
using TalesFromRepoAPI.Core.Models;

namespace TalesFromRepoAPI.Core.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment> GetByIdAsync(Guid id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }

        public async Task<List<Comment>> GetByPostIdAsync(Guid postId)
        {
            return await _commentRepository.GetByPostIdAsync(postId);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            return await _commentRepository.CreateAsync(comment);
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            return await _commentRepository.UpdateAsync(comment);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _commentRepository.DeleteAsync(id);
        }
    }
}