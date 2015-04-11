using System;

namespace IdeasAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Entry Entry { get; set; }
    }
}