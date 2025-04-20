using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace TalesFromRepoAPI.Infrastructure.Data.Entities
{
    [DynamoDBTable("Comments")]
    public class CommentEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey("post-index")]
        public string PostId { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey("user-index")]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string Content { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty]
        public DateTime? UpdatedAt { get; set; }
    }
}