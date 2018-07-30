using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTEC_CL.Comparers
{
	public class MatchComparer : IEqualityComparer<Match>
	{
		public bool Equals(Match x, Match y)
		{
			return x.HomeTeam.Equals(y.HomeTeam, StringComparison.InvariantCultureIgnoreCase) &&
				x.AwayTeam.Equals(y.AwayTeam, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(Match obj)
		{
			return obj.HomeTeam.GetHashCode() ^
				obj.AwayTeam.GetHashCode();
		}
	}
}