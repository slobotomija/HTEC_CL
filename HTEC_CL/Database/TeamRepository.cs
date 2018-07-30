using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace HTEC_CL.Database
{
	public class TeamRepository : ITeamRepository
	{
		IDatabaseProvider dbProvider;

		public TeamRepository(IDatabaseProvider dbp)
		{
			dbProvider = dbp;
		}

		public void Insert(IEnumerable<Team> teams, string group)
		{
			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				using (SQLiteTransaction transaction = dbConn.BeginTransaction())
				{
					foreach (Team team in teams)
					{
						string insertTeam = "INSERT OR REPLACE INTO [Team] VALUES (@rank, @team, @playedGames, @points, @goals, @goalsAgainst, @win, @lose, @draw, @groupName)";
						SQLiteCommand command = new SQLiteCommand(insertTeam, dbConn);
						command.Parameters.Add(new SQLiteParameter("@rank", team.Rank));
						command.Parameters.Add(new SQLiteParameter("@team", team.TeamName));
						command.Parameters.Add(new SQLiteParameter("@playedGames", team.PlayedGames));
						command.Parameters.Add(new SQLiteParameter("@points", team.Points));
						command.Parameters.Add(new SQLiteParameter("@goals", team.Goals));
						command.Parameters.Add(new SQLiteParameter("@goalsAgainst", team.GoalsAgainst));
						command.Parameters.Add(new SQLiteParameter("@win", team.Win));
						command.Parameters.Add(new SQLiteParameter("@lose", team.Lose));
						command.Parameters.Add(new SQLiteParameter("@draw", team.Draw));
						command.Parameters.Add(new SQLiteParameter("@groupName", group));

						command.ExecuteNonQuery();
					}

					transaction.Commit();
				}
			}
		}

		public List<Team> GetTeamsForGroup(string group)
		{
			List<Team> result = new List<Team>();

			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				string selectTeam = "SELECT rank, team, playedGames, points, goals, goalsAgainst, win, lose, draw FROM [Team] WHERE groupName = @groupName";
				SQLiteCommand command = new SQLiteCommand(selectTeam, dbConn);
				command.Parameters.Add(new SQLiteParameter("@groupName", group));
				SQLiteDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					Team team = new Team();
					team.Rank = int.Parse(reader["rank"].ToString());
					team.TeamName = reader["team"].ToString();
					team.PlayedGames = int.Parse(reader["playedGames"].ToString());
					team.Points = int.Parse(reader["points"].ToString());
					team.Goals = int.Parse(reader["goals"].ToString());
					team.GoalsAgainst = int.Parse(reader["goalsAgainst"].ToString());
					team.Win = int.Parse(reader["win"].ToString());
					team.Lose = int.Parse(reader["lose"].ToString());
					team.Draw = int.Parse(reader["draw"].ToString());
					result.Add(team);
				}
			}

			return result;
		}
	}
}