using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using TalesFromRepoAPI.Core.Models;
using TalesFromRepoAPI.Core.Services;
using TalesFromRepo.Lambda.Models.Requests;
using TalesFromRepo.Lambda.Models.Responses;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace TalesFromRepoAPI.Lambda.Functions
{
    public class PostFunctions
    {
        private readonly PostService _postService;

        public PostFunctions()
        {
            var serviceProvider = Startup.BuildServiceProvider();
            _postService = serviceProvider.GetRequiredService<PostService>();
        }

        public async Task<APIGatewayProxyResponse> GetAllPosts(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine($"Executing function: {request}");
            
            try
            {
                var posts = await _postService.GetAllPostsAsync();
                context.Logger.LogLine($"Successfully retreived Posts {posts.Count}");
                
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(posts),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error getting posts: {ex.Message}");
                
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(new ErrorResponse 
                    { 
                        Message = "An error occurred while retrieving posts."
                    }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }

        public async Task<APIGatewayProxyResponse> GetPostById(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Executing GetPostById....");
            try
            {
                context.Logger.LogLine($"Request Body: {request.PathParameters}");
                if (!request.PathParameters.TryGetValue("id", out var idStr) || !Guid.TryParse(idStr, out var id))
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Invalid post ID." }),
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                    };
                }

                context.Logger.LogLine("Getting post from db....");
                var post = await _postService.GetPostByIdAsync(id);
                context.Logger.LogLine($" Successfully retreived Post {post?.Title}");
                if (post == null)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Post not found." }),
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                    };
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(post),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error getting post in GetPostById: {ex.Message}");
                
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(new ErrorResponse 
                    { 
                        Message = "An error occurred while retrieving the post."
                    }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }

        public async Task<APIGatewayProxyResponse> CreatePost(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var createRequest = JsonConvert.DeserializeObject<CreatePostRequest>(request.Body);
                context.Logger.LogLine($"Successfully deserialized request {createRequest?.Title}");
                if (createRequest == null || string.IsNullOrEmpty(createRequest.Title) || string.IsNullOrEmpty(createRequest.Content))
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Title and content are required." }),
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                    };
                }

                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    Title = createRequest.Title,
                    Content = createRequest.Content,
                    Tags = createRequest.Tags ?? new List<string>(),
                    Published = (bool)createRequest.Published
                };

                var createdPost = await _postService.CreatePostAsync(post);

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Body = JsonConvert.SerializeObject(createdPost),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error creating post: {ex.Message}");
                
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(new ErrorResponse 
                    { 
                        Message = "An error occurred while creating the post."
                    }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }

        public async Task<APIGatewayProxyResponse> UpdatePost(APIGatewayProxyRequest request, ILambdaContext lambdaContext) {
            var updateRequest = JsonConvert.DeserializeObject<UpdatePostRequest>(request.Body);
            if (updateRequest == null || string.IsNullOrEmpty(updateRequest.Id) || string.IsNullOrEmpty(updateRequest.Title) || string.IsNullOrEmpty(updateRequest.Content))
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Id, Title and content are required." }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }

            var post = new Post
            {
                Id = Guid.Parse(updateRequest.Id),
                Title = updateRequest.Title,
                Content = updateRequest.Content,
                Tags = updateRequest.Tags ?? new List<string>(),
                Published = (bool)updateRequest.Published
            };

            var updatedPost = await _postService.UpdatePostAsync(post);
            if(updatedPost == null)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Post not found." }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(updatedPost),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

        public async Task<APIGatewayProxyResponse> DeletePost(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Executing DeletePost....");
            try
            {
                if (!request.PathParameters.TryGetValue("id", out var idStr) || !Guid.TryParse(idStr, out var id))
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Invalid post ID." }),
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                    };
                }

                var post = await _postService.GetPostByIdAsync(id);
                if (post == null)
                {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Post not found." }),
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                    };
                }

                var isDeleted = await _postService.DeletePostAsync(id);
                if(!isDeleted) {
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Error deleting post." }),
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                    };
                }
                 
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    Body = null,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error deleting post: {ex.Message}");
                
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(new ErrorResponse 
                    { 
                        Message = "An error occurred while deleting the post."
                    }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }
    }
}