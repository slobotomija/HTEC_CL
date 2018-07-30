using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;

namespace HTEC_CL.Database
{
	public class DatabaseProvider : IDatabaseProvider
	{
		private SQLiteConnection dbConn;
		private string path;

		public DatabaseProvider()
		{
			path = AppDomain.CurrentDomain.BaseDirectory;
		}

		public SQLiteConnection OpenConnection()
		{
			dbConn = new SQLiteConnection("Data Source=" + path + "\\bin\\CL.db3;Version=3;");
			dbConn.Open();
			return dbConn;
		}
	}
}