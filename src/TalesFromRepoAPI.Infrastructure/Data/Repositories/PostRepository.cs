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
    public class PostRepository : IPostRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public PostRepository(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext ?? throw new ArgumentNullException(nameof(dynamoDbContext));
        }

        public async Task<List<Post>> GetAllAsync(bool includeUnpublished = false)
        {
            // If we want published posts only, we need to filter
            if (!includeUnpublished)
            {
                var filter = new ScanFilter();
                filter.AddCondition("Published", ScanOperator.Equal, true);
                
                var scanConfig = new ScanOperationConfig
                {
                    Filter = filter,
                    ConsistentRead = true
                };

                var search = _dynamoDbContext.FromScanAsync<PostEntity>(scanConfig);
                
                var postEntities = new List<PostEntity>();
                do
                {
                    var page = await search.GetNextSetAsync();
                    postEntities.AddRange(page);
                } 
                while (!search.IsDone);

                return postEntities.Select(entity => MapToPost(entity)).ToList();
            }
            else
            {
                // Get all posts without filtering
                var search = _dynamoDbContext.ScanAsync<PostEntity>(new List<ScanCondition>());
                var postEntities = await search.GetRemainingAsync();
                return postEntities.Select(entity => MapToPost(entity)).ToList();
            }
        }

        // public async Task<Post> GetByIdAsync(Guid id)
        // {
        //     var postEntity = await _dynamoDbContext.LoadAsync<PostEntity>(id.ToString());
        //     if (postEntity == null)
        //     {
        //         return null;
        //     }

        //     return MapToPost(postEntity);
        // }

        // public async Task<Post> GetBySlugAsync(string slug)
        // {
        //     // Query the GSI for slug index
        //     var search = _dynamoDbContext.QueryAsync<PostEntity>(
        //         slug,
        //         QueryOperator.Equal,
        //         new List<object> { slug },
        //         new DynamoDBOperationConfig
        //         {
        //             IndexName = "slug-index"
        //         });
            
        //     var posts = await search.GetRemainingAsync();
        //     var postEntity = posts.FirstOrDefault();
            
        //     if (postEntity == null)
        //     {
        //         return null;
        //     }

        //     return MapToPost(postEntity);
        // }

        // public async Task<List<Post>> GetByAuthorAsync(Guid authorId)
        // {
        //     // Query the GSI for author index
        //     var search = _dynamoDbContext.QueryAsync<PostEntity>(
        //         authorId.ToString(),
        //         QueryOperator.Equal,
        //         new List<object> { authorId.ToString() },
        //         new DynamoDBOperationConfig
        //         {
        //             IndexName = "author-index"
        //         });
            
        //     var postEntities = await search.GetRemainingAsync();
        //     return postEntities.Select(entity => MapToPost(entity)).ToList();
        // }

        // public async Task<List<Post>> GetByTagAsync(string tag)
        // {
        //     // DynamoDB doesn't directly support querying by list elements,
        //     // so we'll scan with a filter
        //     var filter = new ScanFilter();
        //     filter.AddCondition("Tags", ScanOperator.Contains, tag);
            
        //     var scanConfig = new ScanOperationConfig
        //     {
        //         Filter = filter
        //     };

        //     var search = _dynamoDbContext.FromScanAsync<PostEntity>(scanConfig);
            
        //     var postEntities = new List<PostEntity>();
        //     do
        //     {
        //         var page = await search.GetNextSetAsync();
        //         postEntities.AddRange(page);
        //     }
        //     while (!search.IsDone);

        //     return postEntities.Select(entity => MapToPost(entity)).ToList();
        // }

        // public async Task<Post> CreateAsync(Post post)
        // {
        //     // Ensure the ID is set
        //     if (post.Id == Guid.Empty)
        //     {
        //         post.Id = Guid.NewGuid();
        //     }

        //     var postEntity = MapToEntity(post);
        //     await _dynamoDbContext.SaveAsync(postEntity);
        //     return MapToPost(postEntity);
        // }

        // public async Task<Post> UpdateAsync(Post post)
        // {
        //     // First, check if the post exists
        //     var existingPost = await GetByIdAsync(post.Id);
        //     if (existingPost == null)
        //     {
        //         throw new KeyNotFoundException($"Post with ID {post.Id} not found.");
        //     }

        //     var postEntity = MapToEntity(post);
        //     await _dynamoDbContext.SaveAsync(postEntity);
        //     return MapToPost(postEntity);
        // }

        // public async Task<bool> DeleteAsync(Guid id)
        // {
        //     var existingPost = await GetByIdAsync(id);
        //     if (existingPost == null)
        //     {
        //         return false;
        //     }

        //     await _dynamoDbContext.DeleteAsync<PostEntity>(id.ToString());
        //     return true;
        // }

        #region Private Helper Methods

        private Post MapToPost(PostEntity entity)
        {
            if (entity == null) return null;

            return new Post
            {
                Id = Guid.Parse(entity.Id),
                Title = entity.Title,
                Content = entity.Content,
                Slug = entity.Slug,
                AuthorId = Guid.Parse(entity.AuthorId),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Tags = entity.Tags ?? new List<string>(),
                Published = entity.Published,
                // Note: We're not loading the Author and Comments here
                // Those would typically be loaded separately or via a join operation
            };
        }

        private PostEntity MapToEntity(Post post)
        {
            if (post == null) return null;

            return new PostEntity
            {
                Id = post.Id.ToString(),
                Title = post.Title,
                Content = post.Content,
                Slug = post.Slug,
                AuthorId = post.AuthorId.ToString(),
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                Tags = post.Tags ?? new List<string>(),
                Published = post.Published
            };
        }

        #endregion
    }
}
