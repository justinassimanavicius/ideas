using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IdeasAPI.Models
{
    public enum EntryStatus
    {
        [Description("Open")]
        Open,
        [Description("In progress")]
        InProgress,
        [Description("Awaiting analysis")]
        AwaitingAnalysis,
        [Description("Awaiting implementation")]
        AwaitingImplementation,
        [Description("Verification")]
        Verification,
        [Description("In analysis")]
        InAnalysis,
        [Description("Resolved")]
        Resolved,
        [Description("Closed")]
        Closed,
        [Description("Trash")]
        Trash
    }
}
