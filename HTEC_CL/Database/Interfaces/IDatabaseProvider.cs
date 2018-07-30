using System.Data.SQLite;

namespace HTEC_CL.Database
{
	public interface IDatabaseProvider
	{
		SQLiteConnection OpenConnection();
	}
}