using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdeasAPI.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public EntrySource Source { get; set; }
        public string Author { get; set; }
        public string Assignee { get; set; }
        public List<string> Followers { get; set; }
        public EntryStatus Status { get; set; }
        public List<UserGroup> SecurityLevel { get; set; }
        public EntryVisibility Visibility { get; set; }
        public EntryPriority Priority { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Vote> Votes { get; set; }
    }
}