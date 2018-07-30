using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTEC_CL.Database
{
	public class DatabaseHandler : IDatabaseHandler
	{
		IGroupRepository groupRepository;
		IMatchRepository matchRepository;
		public DatabaseHandler(IGroupRepository gr, IMatchRepository mr)
		{
			groupRepository = gr;
			matchRepository = mr;
		}

		public List<Group> GetAllGroups()
		{
			return groupRepository.GetAll();
		}

		public List<Group> GetGroups(IEnumerable<string> listOfGroups)
		{
			return groupRepository.Get(listOfGroups);
		}

		public void UpdateMatches(IEnumerable<Match> matches)
		{
			matchRepository.Update(matches);
		}

		public List<Match> GetFilteredMatches(string team, string group)
		{
			return matchRepository.Get(team, group);
		}
	}
}