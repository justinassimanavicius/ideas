using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeasAPI.Models
{
    public class VoteView
    {
        public int Id { get; set; }
        public bool IsPositive { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
    }
}