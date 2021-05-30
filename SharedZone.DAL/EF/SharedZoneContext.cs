using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using SharedZone.DAL.Entities;

namespace SharedZone.DAL.EF
{
	public class SharedZoneContext : DbContext
	{
		public DbSet<Collection> Collections { get; set; }
		public DbSet<Client> Clients { get; set; }
		public DbSet<Hour> Hours { get; set; }
		public DbSet<IFCFileType> IFCFileTypes { get; set; }
		public DbSet<IFCIncludeBoundaty> IFCIncludeBoundaties { get; set; }
		public DbSet<IFCJob> IFCJobs { get; set; }
		public DbSet<JobLaunch> JobLaunches { get; set; }
		public DbSet<Minute> Minutes { get; set; }
		public DbSet<NavisConvertElementsProperty> NavisConvertElementsProperties { get; set; }
		public DbSet<NavisCoordinate> NavisCoordinates { get; set; }
		public DbSet<NavisJob> NavisJobs { get; set; }
		public DbSet<NavisView> NavisViews { get; set; }
		public DbSet<RevitJob> RevitJobs { get; set; }
		public DbSet<RevitModel> RevitModels { get; set; }
		public DbSet<RevitServer> RevitServers { get; set; }
		public DbSet<RevitVersion> RevitVersions { get; set; }
		public DbSet<WeekDay> WeekDays { get; set; }
		public DbSet<ServerLog> ServerLogs { get; set; }
		public DbSet<License> Licenses { get; set; }
		//public DbSet<LicenseUsing> LicenseUsings { get; set; }
		public DbSet<LicenseUsersStatistic> LicenseUsersStatistics { get; set; }

		//public SharedZoneContext()
		//{
		//	Database.SetInitializer(new DbInitializer());
		//}

		public SharedZoneContext(string connectionString) : base(connectionString)
		{
			//Configuration.LazyLoadingEnabled = false;
			//Configuration.AutoDetectChangesEnabled = false;
			Database.SetInitializer(new DbInitializer());
		}

		//для тестов
		public SharedZoneContext(DbConnection conn) : base(conn, true)
		{
			Configuration.LazyLoadingEnabled = false;
			Database.SetInitializer(new DbInitializer());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			/*WeekDaysCollections*/
			modelBuilder.Entity<WeekDay>().HasMany(u => u.Collections)
			   .WithMany(r => r.WeekDays)
			   .Map(r => r.MapLeftKey("WeekDayId")
			   .MapRightKey("CollectionId")
			   .ToTable("WeekDaysCollections"));

			/*RevitModelsCollections*/
			modelBuilder.Entity<RevitModel>().HasMany(u => u.Collections)
			   .WithMany(r => r.RevitModels)
			   .Map(r => r.MapLeftKey("RevitModelId")
			   .MapRightKey("CollectionId")
			   .ToTable("RevitModelsCollections"));

		}

		public class DbInitializer : CreateDatabaseIfNotExists<SharedZoneContext>
		{
			protected override void Seed(SharedZoneContext db)
			{
				/*ADD HOURS*/
				int h = 0;
				db.Hours.Add(new Hour() { Id = 1, Name = "00", HourValue = h });
				db.Hours.Add(new Hour() { Id = 2, Name = "01", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 3, Name = "02", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 4, Name = "03", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 5, Name = "04", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 6, Name = "05", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 7, Name = "06", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 8, Name = "07", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 9, Name = "08", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 10, Name = "09", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 11, Name = "10", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 12, Name = "11", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 13, Name = "12", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 14, Name = "13", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 15, Name = "14", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 16, Name = "15", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 17, Name = "16", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 18, Name = "17", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 19, Name = "18", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 20, Name = "19", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 21, Name = "20", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 22, Name = "21", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 23, Name = "22", HourValue = ++h });
				db.Hours.Add(new Hour() { Id = 24, Name = "23", HourValue = ++h });

				/*ADD MINUTES*/
				db.Minutes.Add(new Minute() { Id = 1, Name = "00", MinuteValue = 0 });
				db.Minutes.Add(new Minute() { Id = 2, Name = "15", MinuteValue = 15 });
				db.Minutes.Add(new Minute() { Id = 3, Name = "30", MinuteValue = 30 });
				db.Minutes.Add(new Minute() { Id = 4, Name = "45", MinuteValue = 45 });

				//*ADD IFCFileTypes*/
				db.IFCFileTypes.Add(new IFCFileType() { Name = "Default" });
				db.IFCFileTypes.Add(new IFCFileType() { Name = "IFC 2x2 (*.ifc)" });
				db.IFCFileTypes.Add(new IFCFileType() { Name = "IFC 2x3 (*.ifc)" });
				db.IFCFileTypes.Add(new IFCFileType() { Name = "IFC BCA ePlan Check (*.ifc)" });
				db.IFCFileTypes.Add(new IFCFileType() { Name = "IFC 2x2 Coordination View " });
				db.IFCFileTypes.Add(new IFCFileType() { Name = "IFC GSA 2010 (*.ifc)" });

				/*ADD IFCIncludeBoundaties*/
				db.IFCIncludeBoundaties.Add(new IFCIncludeBoundaty() { Name = "No" });
				db.IFCIncludeBoundaties.Add(new IFCIncludeBoundaty() { Name = "First Level" });
				db.IFCIncludeBoundaties.Add(new IFCIncludeBoundaty() { Name = "Second Level" });

				/*NavisConvertElementsProerties*/
				db.NavisConvertElementsProperties.Add(new NavisConvertElementsProperty() { Name = "All" });
				db.NavisConvertElementsProperties.Add(new NavisConvertElementsProperty() { Name = "No" });
				db.NavisConvertElementsProperties.Add(new NavisConvertElementsProperty() { Name = "Elements" });

				/*NavisCoordinate*/
				db.NavisCoordinates.Add(new NavisCoordinate() { Name = "Shared" });
				db.NavisCoordinates.Add(new NavisCoordinate() { Name = "Internal" });

				/*NavisViews*/
				db.NavisViews.Add(new NavisView() { Name = "WholeProject" });
				db.NavisViews.Add(new NavisView() { Name = "ChooseView" });

				/*RevitVersions*/
				db.RevitVersions.Add(new RevitVersion() { Name = "Default", Id = 1, IsDefault = true});
				foreach (string v in new List<string> { "2015", "2016", "2017", "2018", "2019", "2020", "2021" })
					db.RevitVersions.Add(new RevitVersion() { Name = v, IsDefault = false });
				/*WeekDays*/
				db.WeekDays.Add(new WeekDay() { Id = 1, Name = "Mo" });
				db.WeekDays.Add(new WeekDay() { Id = 2, Name = "Tu" });
				db.WeekDays.Add(new WeekDay() { Id = 3, Name = "We" });
				db.WeekDays.Add(new WeekDay() { Id = 4, Name = "Th" });
				db.WeekDays.Add(new WeekDay() { Id = 5, Name = "Fr" });
				db.WeekDays.Add(new WeekDay() { Id = 6, Name = "Sa" });
				db.WeekDays.Add(new WeekDay() { Id = 7, Name = "Su" });

				db.SaveChanges();

			}
		}
	}
}
