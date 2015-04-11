using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeasAPI.Models
{
    public class EntryView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public int Vote { get; set; }
        public string Status { get; set; }
        public int Comments { get; set; }
    }
}