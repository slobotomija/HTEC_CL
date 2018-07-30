using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTEC_CL.Models
{
	public class Match
	{
		public string LeagueTitle { get; set; }
		public int Matchday { get; set; }
		public string Group { get; set; }
		public string HomeTeam { get; set; }
		public string AwayTeam { get; set; }
		public string KickoffAt { get; set; }
		public string Score { get; set; }
	}
}