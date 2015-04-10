using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeasAPI.Models
{
    public class User
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Email { get; set; }
        public byte[] Thumbnail { get; set; }
        public DateTime BirthDay { get; set; }
        public string DomainName { get; set; }
        public bool IsModerator { get; set; }
    }
}