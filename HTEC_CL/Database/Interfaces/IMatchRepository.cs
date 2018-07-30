using System.Collections.Generic;
using HTEC_CL.Models;

namespace HTEC_CL.Database
{
	public interface IMatchRepository
	{
		List<Match> Get(IEnumerable<string> listOfGroups);
		List<Match> Get(string team, string group);
		void Insert(IEnumerable<Match> matches);
		void Update(IEnumerable<Match> matches);
	}
}