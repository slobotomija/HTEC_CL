using System.Collections.Generic;
using HTEC_CL.Models;

namespace HTEC_CL.Database
{
	public interface IGroupRepository
	{
		List<Group> Get(IEnumerable<string> listOfGroups);
		List<Group> GetAll();
		void Insert(IEnumerable<Group> groups);
	}
}