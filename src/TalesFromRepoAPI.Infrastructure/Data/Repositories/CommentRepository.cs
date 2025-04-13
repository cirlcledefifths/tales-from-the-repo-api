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
    public class CommentRepository : ICommentRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly DynamoDBContext _context;

        public CommentRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
            _context = new DynamoDBContext(_dynamoDb);
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            var search = _context.ScanAsync<CommentEntity>(new List<ScanCondition>());
            var comments = await search.GetRemainingAsync();
            return comments.Select(c => new Comment
            {
                Id = Guid.Parse(c.Id),
                Content = c.Content,
                PostId = Guid.Parse(c.PostId),
                AuthorId = Guid.Parse(c.AuthorId),
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }

        // public async Task<Comment> GetByIdAsync(Guid id)
        // {
        //     var commentEntity = await _context.LoadAsync<CommentEntity>(id.ToString());
        //     if (commentEntity == null) return null;

        //     return new Comment
        //     {
        //         Id = Guid.Parse(commentEntity.Id),
        //         Content = commentEntity.Content,
        //         PostId = Guid.Parse(commentEntity.PostId),
        //         AuthorId = Guid.Parse(commentEntity.AuthorId),
        //         CreatedAt = commentEntity.CreatedAt,
        //         UpdatedAt = commentEntity.UpdatedAt
        //     };
        // }

        // public async Task<List<Comment>> GetByPostIdAsync(Guid postId)
        // {
        //     var search = _context.QueryAsync<CommentEntity>(postId.ToString());
        //     var comments = await search.GetRemainingAsync();
        //     return comments.Select(c => new Comment
        //     {
        //         Id = Guid.Parse(c.Id),
        //         Content = c.Content,
        //         PostId = Guid.Parse(c.PostId),
        //         AuthorId = Guid.Parse(c.AuthorId),
        //         CreatedAt = c.CreatedAt,
        //         UpdatedAt = c.UpdatedAt
        //     }).ToList();
        // }

        // public async Task<Comment> CreateAsync(Comment comment)
        // {
        //     var commentEntity = new CommentEntity
        //     {
        //         Id = comment.Id.ToString(),
        //         Content = comment.Content,
        //         PostId = comment.PostId.ToString(),
        //         AuthorId = comment.AuthorId.ToString(),
        //         CreatedAt = DateTime.UtcNow,
        //         UpdatedAt = null
        //     };

        //     await _context.SaveAsync(commentEntity);
        //     return comment;
        // }

        // public async Task<Comment> UpdateAsync(Comment comment)
        // {
        //     var commentEntity = new CommentEntity
        //     {
        //         Id = comment.Id.ToString(),
        //         Content = comment.Content,
        //         PostId = comment.PostId.ToString(),
        //         AuthorId = comment.AuthorId.ToString(),
        //         CreatedAt = comment.CreatedAt,
        //         UpdatedAt = DateTime.UtcNow
        //     };

        //     await _context.SaveAsync(commentEntity);
        //     return comment;
        // }

        // public async Task<bool> DeleteAsync(Guid id)
        // {
        //     var commentEntity = await _context.LoadAsync<CommentEntity>(id.ToString());
        //     if (commentEntity == null) return false;

        //     await _context.DeleteAsync(commentEntity);
        //     return true;
        // }
    }
}