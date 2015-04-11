using System;

namespace IdeasAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Edited { get; set; }
    }
}