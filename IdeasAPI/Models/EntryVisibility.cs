using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IdeasAPI.Models
{
    public enum EntryVisibility
    {
        [Description("Viešas")]
        Public,
        [Description("Juodraštis")]
        Draft,
        [Description("Paslėptas")]
        Hidden
    }
}
