using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace TalesFromRepoAPI.Infrastructure.Data.Entities
{
    [DynamoDBTable("Posts")]
    public class PostEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty]
        public string Content { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey("slug-index")]
        public string Slug { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey("author-index")]
        public string AuthorId { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty]
        public DateTime? UpdatedAt { get; set; }

        [DynamoDBProperty]
        public List<string> Tags { get; set; }

        [DynamoDBProperty]
        public bool Published { get; set; }
    }
}