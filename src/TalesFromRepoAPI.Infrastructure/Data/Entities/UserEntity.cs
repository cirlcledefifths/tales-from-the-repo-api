using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace TalesFromRepoAPI.Infrastructure.Data.Entities
{
    [DynamoDBTable("Users")]
    public class UserEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string Username { get; set; }

        [DynamoDBProperty]
        public string Email { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty]
        public DateTime UpdatedAt { get; set; }

        [DynamoDBProperty]
        public List<string> PostIds { get; set; } = new List<string>();
    }
}