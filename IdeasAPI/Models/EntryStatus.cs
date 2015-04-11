using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IdeasAPI.Models
{
    public enum EntryStatus
    {
        [Description("Laukia atsakymo")]
        Open,
        [Description("Sprendžiamas")]
        InProgress,
        [Description("Laukiama analizės")]
        AwaitingAnalysis,
        [Description("Laukiama įgyvendinimo")]
        AwaitingImplementation,
        [Description("Vyksta verifikacija")]
        Verification,
        [Description("Vyksta analizė")]
        InAnalysis,
        [Description("Išspręstas")]
        Resolved,
        [Description("Uždarytas")]
        Closed,
        [Description("Ištrintas")]
        Trash
    }
}
