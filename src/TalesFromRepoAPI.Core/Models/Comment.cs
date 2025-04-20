using System;
using System.Collections.Generic;

namespace TalesFromRepoAPI.Core.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public Post Post { get; set; }
        public User Author { get; set; }
        public List<bool> Likes { get; set; } = new List<bool>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}