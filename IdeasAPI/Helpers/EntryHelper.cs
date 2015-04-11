using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using IdeasAPI.Models;

namespace IdeasAPI.Helpers
{
    public static class EntryHelper
    {
        public static int GetVotes(List<Vote> votes)
        {
            return votes != null ? votes.Count : 0;
        }

        public static bool? UserVoteResult(IIdentity id, List<Vote> votes)
        {
            if (votes == null) return null;

            var userVote = votes.FirstOrDefault(x => x.Author == UserHelper.GetUserNameFromComplexUsername(id.Name));

            if (userVote == null) return null;

            return userVote.IsPositive;
        }
    }
}