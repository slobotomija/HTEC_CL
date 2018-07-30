using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace HTEC_CL.Database
{
	public class GroupRepository : IGroupRepository
	{
		ITeamRepository teamRepository;
		IDatabaseProvider dbProvider;

		public GroupRepository(ITeamRepository itr, IDatabaseProvider dbp)
		{
			teamRepository = itr;
			dbProvider = dbp;
		}

		public void Insert(IEnumerable<Group> groups)
		{
			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				using (SQLiteTransaction transaction = dbConn.BeginTransaction())
				{
					foreach (Group group in groups)
					{
						string insertGroup = "INSERT OR REPLACE INTO [Group] values (@leagueTitle, @matchday, @group)";
						SQLiteCommand command = new SQLiteCommand(insertGroup, dbConn);
						command.Parameters.Add(new SQLiteParameter("@leagueTitle", group.LeagueTitle));
						command.Parameters.Add(new SQLiteParameter("@matchday", group.Matchday));
						command.Parameters.Add(new SQLiteParameter("@group", group.GroupName));

						command.ExecuteNonQuery();
					}

					transaction.Commit();
				}
			}

			//TeamRepository tr = new TeamRepository();
			foreach (Group group in groups)
			{
				teamRepository.Insert(group.Standing, group.GroupName);
			}
		}

		public List<Group> GetAll()
		{
			List<Group> result = new List<Group>();
			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				string selectGroups = "SELECT leagueTitle, matchday, name FROM [Group]";
				SQLiteCommand command = new SQLiteCommand(selectGroups, dbConn);
				SQLiteDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					Group group = new Group();
					group.LeagueTitle = reader["leagueTitle"].ToString();
					group.Matchday = int.Parse(reader["matchday"].ToString());
					group.GroupName = reader["name"].ToString();
					result.Add(group);
				}
			}

			//TeamRepository tr = new TeamRepository();
			foreach(Group g in result)
			{
				g.Standing = teamRepository.GetTeamsForGroup(g.GroupName);
			}

			return result;
		}

		public List<Group> Get(IEnumerable<string> listOfGroups)
		{
			List<Group> result = new List<Group>();
			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				string selectGroups = string.Format("SELECT leagueTitle, matchday, name FROM [Group] WHERE name IN ('{0}')", string.Join("', '", listOfGroups.ToArray()));
				SQLiteCommand command = new SQLiteCommand(selectGroups, dbConn);
				SQLiteDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					Group group = new Group();
					group.LeagueTitle = reader["leagueTitle"].ToString();
					group.Matchday = int.Parse(reader["matchday"].ToString());
					group.GroupName = reader["name"].ToString();
					result.Add(group);
				}

				dbConn.Close();
			}

			//TeamRepository tr = new TeamRepository();
			foreach (Group g in result)
			{
				g.Standing = teamRepository.GetTeamsForGroup(g.GroupName);
			}

			return result;
		}
	}
}