using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeasAPI.Helpers
{
    public static class EntryHelper
    {
        public static int GetVotes(List<string> upVoters, List<string> downVoters)
        {
            if (upVoters == null && downVoters == null) return 0;

            if (upVoters != null && downVoters == null) return upVoters.Count;

            if (upVoters == null) return downVoters.Count;

            return upVoters.Count - downVoters.Count;
        }
    }
}