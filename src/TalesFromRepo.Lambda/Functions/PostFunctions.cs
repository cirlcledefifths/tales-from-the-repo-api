using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using TalesFromRepoAPI.Core.Models;
using TalesFromRepoAPI.Core.Services;
// using TalesFromRepoAPI.Lambda.Models.Requests;
using TalesFromRepoAPI.Lambda.Models.Responses;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
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

        // public async Task<APIGatewayProxyResponse> CreatePost(APIGatewayProxyRequest request, ILambdaContext context)
        // {
        //     try
        //     {
        //         var createRequest = JsonConvert.DeserializeObject<CreatePostRequest>(request.Body);
                
        //         if (createRequest == null || string.IsNullOrEmpty(createRequest.Title) || string.IsNullOrEmpty(createRequest.Content))
        //         {
        //             return new APIGatewayProxyResponse
        //             {
        //                 StatusCode = (int)HttpStatusCode.BadRequest,
        //                 Body = JsonConvert.SerializeObject(new ErrorResponse { Message = "Title and content are required." }),
        //                 Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        //             };
        //         }

        //         // In a real app, get the user ID from auth context
        //         var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        //         var post = new Post
        //         {
        //             Title = createRequest.Title,
        //             Content = createRequest.Content,
        //             AuthorId = userId,
        //             Tags = createRequest.Tags ?? new List<string>(),
        //             Published = createRequest.Published
        //         };

        //         var createdPost = await _postService.CreatePostAsync(post);

        //         return new APIGatewayProxyResponse
        //         {
        //             StatusCode = (int)HttpStatusCode.Created,
        //             Body = JsonConvert.SerializeObject(createdPost),
        //             Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         context.Logger.LogLine($"Error creating post: {ex.Message}");
                
        //         return new APIGatewayProxyResponse
        //         {
        //             StatusCode = (int)HttpStatusCode.InternalServerError,
        //             Body = JsonConvert.SerializeObject(new ErrorResponse 
        //             { 
        //                 Message = "An error occurred while creating the post."
        //             }),
        //             Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        //         };
        //     }
        // }
    }
}