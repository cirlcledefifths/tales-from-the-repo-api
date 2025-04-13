using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.DynamoDBv2.DataModel;
using TalesFromRepoAPI.Core.Interfaces;
using TalesFromRepoAPI.Core.Services;
using TalesFromRepoAPI.Infrastructure.Data.Repositories;

namespace TalesFromRepoAPI.Lambda
{
    public static class Startup
    {
        public static ServiceProvider BuildServiceProvider()
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            // Set up dependency injection
            var serviceCollection = new ServiceCollection();

            // Add AWS services
            serviceCollection.AddDefaultAWSOptions(new AWSOptions
            {
                Region = Amazon.RegionEndpoint.USEast1 // Change to your region
            });
            serviceCollection.AddAWSService<IAmazonDynamoDB>();

            // Add application services
            serviceCollection.AddScoped<IPostRepository, PostRepository>();
            serviceCollection.AddScoped<PostService>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<UserService>();
            serviceCollection.AddScoped<ICommentRepository, CommentRepository>();
            serviceCollection.AddScoped<CommentService>();
            serviceCollection.AddScoped<IDynamoDBContext, DynamoDBContext>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}