using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTEC_CL.Processors
{
	public class TeamComparer : IComparer<Team>
	{
		public int Compare(Team x, Team y)
		{
			if (x.Points < y.Points)
				return 1;
			if (x.Points > y.Points)
				return -1;
			else
			{
				if (x.Goals < y.Goals)
					return 1;
				if (x.Goals > y.Goals)
					return -1;
				else
				{
					if (x.GoalDifference < y.GoalDifference)
						return 1;
					if (x.GoalDifference > y.GoalDifference)
						return -1;
					else
						return 0;
				}
			}
		}
	}
}