using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeasAPI.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public EntrySource Source { get; set; }
        public User Author { get; set; }
        public User Assignee { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Upvoters { get; set; }
        public List<User> Downvoters { get; set; }
        public EntryStatus Status { get; set; }
        public List<UserGroup> SecurityLevel { get; set; }
        public EntryVisibility Visibility { get; set; }
        public EntryPriority Priority { get; set; }
    }
}