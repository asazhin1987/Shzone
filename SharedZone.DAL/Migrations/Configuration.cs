using System.Data.Entity.Migrations;

namespace SharedZone.DAL.Migrations
{
	public sealed class Configuration : DbMigrationsConfiguration<EF.SharedZoneContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
			ContextKey = "SharedZone.DAL.EF.SharedZoneContext";

		}

		protected override void Seed(EF.SharedZoneContext context)
		{

		}
	}
}
