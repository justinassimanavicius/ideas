using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeasAPI.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public bool IsPositive { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public Entry Entry { get; set; }
    }
}