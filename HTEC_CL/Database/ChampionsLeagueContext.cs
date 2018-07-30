using HTEC_CL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace HTEC_CL.Database
{
	public class ChampionsLeagueContext : DbContext
	{
		public ChampionsLeagueContext() :
            base(new SQLiteConnection("Data Source=:memory:;Version=3;New=True;"), true)
        {

		}
		
	
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Team> Teams { get; set; }
	
	}
}