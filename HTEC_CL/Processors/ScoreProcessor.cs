using HTEC_CL.Comparers;
using HTEC_CL.Database;
using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace HTEC_CL.Processors
{
	public class ScoreProcessor : IMatchProcessor
	{
		//Dictionary<string, Team> teamScores = new Dictionary<string, Team>();
		private Dictionary<string, Dictionary<string, Team>> teamScores;
		private IGroupRepository groupRepository;
		private IMatchRepository matchRepository;

		public ScoreProcessor(IGroupRepository igr, IMatchRepository imr)
		{
			teamScores = new Dictionary<string, Dictionary<string, Team>>();
			groupRepository = igr;
			matchRepository = imr;
		}

		public IEnumerable<Group> ProcessMatches(IEnumerable<Match> matches, bool update)
		{
			var distinctMatches = matches.Distinct(new MatchComparer());
			if (!update)
			{
				matchRepository.Insert(distinctMatches);
				Init(update);
			}

			List<Group> rez = new List<Group>();
			Dictionary<string, List<Match>> matchesPerGroup = distinctMatches.GroupBy(m => m.Group).ToDictionary(m => m.Key, m => m.ToList());

			foreach(KeyValuePair<string, List<Match>> groupAndMatches in matchesPerGroup)
			{
				if (!teamScores.ContainsKey(groupAndMatches.Key))
				{
					teamScores.Add(groupAndMatches.Key, new Dictionary<string, Team>());
				}

				Group gr = new Group();
				gr.GroupName = groupAndMatches.Key;
				gr.Standing = ProcessGroupMatches(groupAndMatches.Value, groupAndMatches.Key).ToList();

				gr.LeagueTitle = groupAndMatches.Value.Select(m => m.LeagueTitle).First();
				gr.Matchday = groupAndMatches.Value.Select(m => m.Matchday).Max();

				rez.Add(gr);
			}

			groupRepository.Insert(rez);
			return rez;
		}

		private IEnumerable<Team> ProcessGroupMatches(IEnumerable<Match> matches, string group)
		{
			IEnumerable<string> ekipe = matches.Select(m => m.HomeTeam).Concat(matches.Select(m => m.AwayTeam)).Distinct().ToList();

			foreach (string teamName in ekipe)
			{
				if(!teamScores[group].ContainsKey(teamName))
				{
					Team tim = new Team() { TeamName = teamName };
					teamScores[group][teamName] = tim;
				}
			}

			foreach (Match match in matches)
			{
				ProcessScore(match);
			}

			List<Team> table = teamScores[group].Values.ToList();
			table.Sort(new TeamComparer());

			for (int i = 1; i <= table.Count; i++)
			{
				table[i - 1].Rank = i;
			}

			return table;
		}

		private void ProcessScore(Match m)
		{
			try
			{
				string[] scores = m.Score.Split(':');
				if (scores.Length == 2)
				{
					int homeGoals = int.Parse(scores[0]);
					int awayGoals = int.Parse(scores[1]);

					teamScores[m.Group][m.HomeTeam].PlayMatch(homeGoals, awayGoals);
					teamScores[m.Group][m.AwayTeam].PlayMatch(awayGoals, homeGoals);
				}
			}
			catch (Exception)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest);
			}
		}

		public void UpdateScores(IEnumerable<Match> matches)
		{
			List<Match> updatedMatches = matchRepository.Get(matches.Select(m => m.Group).Distinct());
			ProcessMatches(updatedMatches, true);
		}

		private void Init(bool update)
		{
			List<Group> groups = groupRepository.GetAll();
			foreach (Group g in groups)
			{
				teamScores[g.GroupName] = new Dictionary<string, Team>();
				foreach (Team t in g.Standing)
				{
					if (update)
					{
						teamScores[g.GroupName].Add(t.TeamName, new Team() { TeamName = t.TeamName });
					}
					else
					{
						teamScores[g.GroupName].Add(t.TeamName, new Team() { TeamName = t.TeamName, Points = t.Points, Goals = t.Goals, GoalsAgainst = t.GoalsAgainst,
																			PlayedGames = t.PlayedGames, Win = t.Win, Draw = t.Draw, Lose = t.Lose, Rank = t.Rank});
					}
				
				}
			}
		}
	}
}