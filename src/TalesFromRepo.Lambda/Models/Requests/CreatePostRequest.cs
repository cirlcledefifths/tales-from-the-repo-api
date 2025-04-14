using System;
using System.Collections.Generic;

namespace TalesFromRepo.Lambda.Models.Requests
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        // public string Author { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool Published { get; set; } = false;
    }
}