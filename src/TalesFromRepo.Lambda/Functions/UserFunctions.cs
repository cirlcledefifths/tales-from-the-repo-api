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
    public class UserFunctions {
        private readonly UserService _userService;

        public UserFunctions() {
            var serviceProvider = Startup.BuildServiceProvider();
            _userService = serviceProvider.GetRequiredService<UserService>();
        }

        public async Task<APIGatewayProxyResponse> GetAllUsers(APIGatewayProxyRequest request, ILambdaContext context) {
            try {
                var users = await _userService.GetAllAsync();
                return new APIGatewayProxyResponse {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = JsonConvert.SerializeObject(users)
                };
            } catch (Exception ex) {
                context.Logger.LogError($"Error: {ex.Message}");
                return new APIGatewayProxyResponse {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(new { message = "Internal Server Error" })
                };
            }
        }

        public async Task<APIGatewayProxyResponse> CreateUser(APIGatewayProxyRequest request, ILambdaContext context) {
            try {
                var userRequest = JsonConvert.DeserializeObject<CreateUserRequest>(request.Body);
                if (userRequest == null || string.IsNullOrEmpty(userRequest.Username) || string.IsNullOrEmpty(userRequest.Email)) {
                    return new APIGatewayProxyResponse {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Body = JsonConvert.SerializeObject(new { message = "Invalid request data" })
                    };
                };

                var user = new User {
                    Username = userRequest.Username,
                    Email = userRequest.Email,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var newUser = await _userService.CreateAsync(user);
                return new APIGatewayProxyResponse {
                    StatusCode = (int)HttpStatusCode.Created,
                    Body = JsonConvert.SerializeObject(newUser)
                };    
            } catch (Exception ex) {
                context.Logger.LogError($"Error: {ex.Message}");
                return new APIGatewayProxyResponse {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(new { message = "Internal Server Error" })
                };
            }
        }
    }
}