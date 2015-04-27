using System;

namespace IdeasAPI.Models
{
    public class CommentView
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public byte[] AuthorThumbnail { get; set; }
    }
}