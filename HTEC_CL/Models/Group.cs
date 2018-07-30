using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTEC_CL.Models
{
	public class Group
	{
		public string LeagueTitle { get; set; }
		public int Matchday { get; set; }
		public string GroupName { get; set; }
		public List<Team> Standing { get; set; }
		
	}
}