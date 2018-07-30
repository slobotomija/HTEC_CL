using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTEC_CL.Database
{
	public interface IDatabaseHandler
	{
		List<Group> GetAllGroups();
		List<Group> GetGroups(IEnumerable<string> listOfGroups);
		void UpdateMatches(IEnumerable<Match> matches);
		List<Match> GetFilteredMatches(string team, string group);
	}
}