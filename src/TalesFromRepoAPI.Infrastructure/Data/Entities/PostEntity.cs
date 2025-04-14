using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace TalesFromRepoAPI.Infrastructure.Data.Entities
{
    [DynamoDBTable("Posts")]
    public class PostEntity
    {
        [DynamoDBHashKey] // Partition Key
        public string Id { get; set; }

        [DynamoDBRangeKey] // Sort Key
        public string Title { get; set; }

        [DynamoDBProperty]
        public string Content { get; set; }

        [DynamoDBProperty]
        public string Slug { get; set; }

        [DynamoDBProperty]
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