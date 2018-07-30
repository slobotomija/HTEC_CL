using System.Collections.Generic;
using HTEC_CL.Models;

namespace HTEC_CL.Database
{
	public interface ITeamRepository
	{
		List<Team> GetTeamsForGroup(string group);
		void Insert(IEnumerable<Team> teams, string group);
	}
}