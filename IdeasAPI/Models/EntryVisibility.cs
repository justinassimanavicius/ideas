using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IdeasAPI.Models
{
    public enum EntryVisibility
    {
        [Description("Public")]
        Public,
        [Description("Draft")]
        Draft,
        [Description("Hidden")]
        Hidden
    }
}
