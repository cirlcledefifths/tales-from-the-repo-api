using System;
using System.Collections.Generic;
using TalesFromRepoAPI.Core.Models;

namespace TalesFromRepoAPI.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}