using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTEC_CL.Processors
{
	public interface IMatchProcessor
	{
		IEnumerable<Group> ProcessMatches(IEnumerable<Match> matches, bool update);
		void UpdateScores(IEnumerable<Match> matches);
	}
}