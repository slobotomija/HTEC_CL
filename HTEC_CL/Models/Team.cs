using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTEC_CL.Models
{
	public class Team
	{
		public int Rank { get; set; }
		public string TeamName { get; set; }
		public int PlayedGames { get; set; }
		public int Points { get; set; }
		public int Goals { get; set; }
		public int GoalsAgainst { get; set; }
		public int GoalDifference { get { return Goals - GoalsAgainst; } }
		public int Win { get; set; }
		public int Lose { get; set; }
		public int Draw { get; set; }

		public void PlayMatch(int goalsScored, int goalsConc)
		{
			Goals += goalsScored;
			GoalsAgainst += goalsConc;
			PlayedGames++;
			if (goalsScored > goalsConc)
			{
				Points += 3;
				Win++;
			}
			else if (goalsScored == goalsConc)
			{
				Points += 1;
				Draw++;
			}
			else
			{
				Lose++;
			}
		}

	}
}