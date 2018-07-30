using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;

namespace HTEC_CL.Database
{
	public class MatchRepository : IMatchRepository
	{
		IDatabaseProvider dbProvider;
		
		public MatchRepository(IDatabaseProvider dbp)
		{
			dbProvider = dbp;
		}

		public void Insert(IEnumerable<Match> matches)
		{
			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				using (SQLiteTransaction transaction = dbConn.BeginTransaction())
				{
					foreach (Match match in matches)
					{
						try
						{
							string insertMatch = "INSERT INTO [Match] VALUES (@leagueTitle, @matchday, @group, @homeTeam, @awayTeam, @kickoffAt, @score)";
							SQLiteCommand command = new SQLiteCommand(insertMatch, dbConn);
							command.Parameters.Add(new SQLiteParameter("@leagueTitle", match.LeagueTitle));
							command.Parameters.Add(new SQLiteParameter("@matchday", match.Matchday));
							command.Parameters.Add(new SQLiteParameter("@group", match.Group));
							command.Parameters.Add(new SQLiteParameter("@homeTeam", match.HomeTeam));
							command.Parameters.Add(new SQLiteParameter("@awayTeam", match.AwayTeam));
							command.Parameters.Add(new SQLiteParameter("@kickoffAt", match.KickoffAt));
							command.Parameters.Add(new SQLiteParameter("@score", match.Score));

							command.ExecuteNonQuery();
						}
						catch (Exception)
						{
							throw new HttpResponseException(HttpStatusCode.BadRequest);
						}
					}

					transaction.Commit();
				}
			}
		}

		public void Update(IEnumerable<Match> matches)
		{
			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				using (SQLiteTransaction transaction = dbConn.BeginTransaction())
				{
					foreach (Match match in matches)
					{
						string insertMatch = "UPDATE [Match] SET score = @score WHERE homeTeam = @homeTeam AND awayTeam = @awayTeam AND [group] = @group";
						SQLiteCommand command = new SQLiteCommand(insertMatch, dbConn);
						command.Parameters.Add(new SQLiteParameter("@group", match.Group));
						command.Parameters.Add(new SQLiteParameter("@homeTeam", match.HomeTeam));
						command.Parameters.Add(new SQLiteParameter("@awayTeam", match.AwayTeam));
						command.Parameters.Add(new SQLiteParameter("@score", match.Score));

						command.ExecuteNonQuery();
					}

					transaction.Commit();
				}
			}
		}

		public List<Match> Get(IEnumerable<string> listOfGroups)
		{
			List<Match> result = new List<Match>();
			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				string selectMatches = string.Format("SELECT leagueTitle, matchday, [group], homeTeam, awayTeam, kickOffAt, score FROM Match WHERE [group] IN ('{0}')", string.Join("', '", listOfGroups.ToArray()));
				SQLiteCommand command = new SQLiteCommand(selectMatches, dbConn);
				SQLiteDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					Match match = new Match();
					match.LeagueTitle = reader["leagueTitle"].ToString();
					match.Matchday = int.Parse(reader["matchday"].ToString());
					match.Group = reader["group"].ToString();
					match.HomeTeam = reader["homeTeam"].ToString();
					match.AwayTeam = reader["awayTeam"].ToString();
					match.KickoffAt = reader["kickOffAt"].ToString();
					match.Score = reader["score"].ToString();
					result.Add(match);
				}

				dbConn.Close();
			}

			return result;
		}

		public List<Match> Get(string team, string group)
		{
			List<Match> result = new List<Match>();
			using (SQLiteConnection dbConn = dbProvider.OpenConnection())
			{
				string select = GetSelectQuery(team, group);

				SQLiteCommand command = new SQLiteCommand(select, dbConn);
				if (team != null) command.Parameters.Add(new SQLiteParameter("@team", team));
				if (group != null) command.Parameters.Add(new SQLiteParameter("@group", group));
				SQLiteDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					Match match = new Match();
					match.LeagueTitle = reader["leagueTitle"].ToString();
					match.Matchday = int.Parse(reader["matchday"].ToString());
					match.Group = reader["group"].ToString();
					match.HomeTeam = reader["homeTeam"].ToString();
					match.AwayTeam = reader["awayTeam"].ToString();
					match.KickoffAt = reader["kickOffAt"].ToString();
					match.Score = reader["score"].ToString();
					result.Add(match);
				}

				dbConn.Close();
			}

			return result;
		}

		private string GetSelectQuery(string team, string group)
		{
			StringBuilder query = new StringBuilder("SELECT leagueTitle, matchday, [group], homeTeam, awayTeam, kickOffAt, score FROM Match");

			if (team != null || group != null)
			{
				query.Append(" WHERE ");
			}

			if (team != null )
			{
				query.Append("(homeTeam = @team OR awayTeam = @team)");

				if (group!=null) query.Append(" AND ");
			}
			if (group != null)
			{
				query.Append("[group] = @group ");
			}


			return query.ToString();
		}
	}
}