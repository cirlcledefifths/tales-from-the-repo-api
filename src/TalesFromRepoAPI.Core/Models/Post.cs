using System;
using System.Collections.Generic;

namespace TalesFromRepoAPI.Core.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public bool Published { get; set; }
    }
}