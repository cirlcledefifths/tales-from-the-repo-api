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

namespace TalesFromRepoAPI.Lambda.Functions {
    public class CommentFunctions {
        private readonly CommentService _commentService;

        public CommentFunctions() {
            var serviceProvider = Startup.BuildServiceProvider();
            _commentService = serviceProvider.GetRequiredService<CommentService>();
        }

        public async Task<APIGatewayProxyResponse> GetAllComments(APIGatewayProxyRequest request, ILambdaContext context) {
            try {
                var comments = await _commentService.GetAllAsync();
                return new APIGatewayProxyResponse {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(comments)
                };
            } catch(Exception ex) {
                context.Logger.LogError($"Error: {ex.Message}");
                return new APIGatewayProxyResponse {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(new { message = "Internal Server Error" })
                };
            }
        }
    }
}