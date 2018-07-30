using HTEC_CL.Database;
using HTEC_CL.Models;
using HTEC_CL.Processors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HTEC_CL.Controllers
{
    public class MatchController : ApiController
    {
		IMatchProcessor processor;
		IDatabaseHandler dbHandler;
		public MatchController(IMatchProcessor matchProcessor, IDatabaseHandler databaseHandler)
		{
			processor = matchProcessor;
			dbHandler = databaseHandler;
		}

		// GET: api/Match
		[Route("api/groups")]
		public List<Group> Get()
        {
			return dbHandler.GetAllGroups();
        }

        // GET: api/Match/5
		[Route("api/groups/{id}")]
        public List<Group> Get(string id)
        {
			List<string> groupIds = id.Split(',').ToList();
			return dbHandler.GetGroups(groupIds);
		}


		[Route("api/matches/{q}")]
		public List<Match> GetMatches(string q)
		{
			
			var query = HttpUtility.ParseQueryString(q);

			var team = query.Get("team");
			var group = query.Get("group");
			return dbHandler.GetFilteredMatches(team, group);
		}

		// POST: api/Match
		public IEnumerable<Group> Post([FromBody] List<Match> value)
        {
			return processor.ProcessMatches(value, false);
		}

        // PUT: api/Match/5
        public void Put([FromBody]List<Match> matches)
        {
			dbHandler.UpdateMatches(matches);
			processor.UpdateScores(matches);
        }

        // DELETE: api/Match/5
        public void Delete(int id)
        {
        }
    }
}
