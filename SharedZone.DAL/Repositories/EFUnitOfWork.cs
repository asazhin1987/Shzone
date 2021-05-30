using SharedZone.DAL.EF;
using SharedZone.DAL.Entities;
using SharedZone.DAL.Interfaces;
using System;

namespace SharedZone.DAL.Repositories
{
	public class EFUnitOfWork : IUnitOfWork
	{
		readonly SharedZoneContext db;

		ClientRepository clientRepository;
		CollectionRepository collectionRepository;
		HourRepository hourRepository;
		IFCFileTypeRepository iFCFileTypeRepository;
		IFCIncludeBoundatyRepository iFCIncludeBoundatyRepository;
		IFCJobRepository iFCJobRepository;
		JobLaunchRepository jobLaunchRepository;
		MinuteRepository minuteRepository;
		NavisConvertElementsPropertyRepository navisConvertElementsPropertyRepository;
		NavisCoordinateRepository navisCoordinateRepository;
		NavisJobRepository navisJobRepository;
		NavisViewRepository navisViewRepository;
		RevitJobRepository revitJobRepository;
		RevitModelRepository revitModelRepository;
		RevitServerRepository revitServerRepository;
		RevitVersionRepository revitVersionRepository;
		WeekDayRepository weekDayRepository;
		ServerLogRepository serverLogRepository;
		LicenseRepository licenseRepository;
		LicenseStatisticRepository licenseStatisticRepository;
		SqlInjector sqlInjector;

		public EFUnitOfWork(string connectionString)
		{
			db = new SharedZoneContext(connectionString);
		}

		public EFUnitOfWork(SharedZoneContext context)
		{
			db = context;
		}


		public IRepository<Client> Clients
		{
			get
			{
				if (clientRepository == null)
					clientRepository = new ClientRepository(db);
				return clientRepository;
			}
		}
		public IRepository<Collection> Collections
		{
			get
			{
				if (collectionRepository == null)
					collectionRepository = new CollectionRepository(db);
				return collectionRepository;
			}
		}
		public IReadOnlyRepository<Hour> Hours
		{
			get
			{
				if (hourRepository == null)
					hourRepository = new HourRepository(db);
				return hourRepository;
			}
		}
		public IReadOnlyRepository<IFCFileType> IFCFileTypes
		{
			get
			{
				if (iFCFileTypeRepository == null)
					iFCFileTypeRepository = new IFCFileTypeRepository(db);
				return iFCFileTypeRepository;
			}
		}
		public IReadOnlyRepository<IFCIncludeBoundaty> IFCIncludeBoundaties
		{
			get
			{
				if (iFCIncludeBoundatyRepository == null)
					iFCIncludeBoundatyRepository = new IFCIncludeBoundatyRepository(db);
				return iFCIncludeBoundatyRepository;
			}
		}
		public IRepository<IFCJob> IFCJobs
		{
			get
			{
				if (iFCJobRepository == null)
					iFCJobRepository = new IFCJobRepository(db);
				return iFCJobRepository;
			}
		}
		public IRepository<JobLaunch> JobLaunches
		{
			get
			{
				if (jobLaunchRepository == null)
					jobLaunchRepository = new JobLaunchRepository(db);
				return jobLaunchRepository;
			}
		}
		public IReadOnlyRepository<Minute> Minutes
		{
			get
			{
				if (minuteRepository == null)
					minuteRepository = new MinuteRepository(db);
				return minuteRepository;
			}
		}
		public IReadOnlyRepository<NavisConvertElementsProperty> NavisConvertElementsProperties
		{
			get
			{
				if (navisConvertElementsPropertyRepository == null)
					navisConvertElementsPropertyRepository = new NavisConvertElementsPropertyRepository(db);
				return navisConvertElementsPropertyRepository;
			}
		}
		public IReadOnlyRepository<NavisCoordinate> NavisCoordinates
		{
			get
			{
				if (navisCoordinateRepository == null)
					navisCoordinateRepository = new NavisCoordinateRepository(db);
				return navisCoordinateRepository;
			}
		}
		public IRepository<NavisJob> NavisJobs
		{
			get
			{
				if (navisJobRepository == null)
					navisJobRepository = new NavisJobRepository(db);
				return navisJobRepository;
			}
		}
		public IReadOnlyRepository<NavisView> NavisViews
		{
			get
			{
				if (navisViewRepository == null)
					navisViewRepository = new NavisViewRepository(db);
				return navisViewRepository;
			}
		}
		public IRepository<RevitJob> RevitJobs
		{
			get
			{
				if (revitJobRepository == null)
					revitJobRepository = new RevitJobRepository(db);
				return revitJobRepository;
			}
		}
		public IRepository<RevitModel> RevitModels
		{
			get
			{
				if (revitModelRepository == null)
					revitModelRepository = new RevitModelRepository(db);
				return revitModelRepository;
			}
		}
		public IRepository<RevitServer> RevitServers
		{
			get
			{
				if (revitServerRepository == null)
					revitServerRepository = new RevitServerRepository(db);
				return revitServerRepository;
			}
		}
		public IRepository<RevitVersion> RevitVersions
		{
			get
			{
				if (revitVersionRepository == null)
					revitVersionRepository = new RevitVersionRepository(db);
				return revitVersionRepository;
			}
		}
		public IReadOnlyRepository<WeekDay> WeekDays
		{
			get
			{
				if (weekDayRepository == null)
					weekDayRepository = new WeekDayRepository(db);
				return weekDayRepository;
			}
		}

		public IRepository<ServerLog> ServerLogs
		{
			get
			{
				if (serverLogRepository == null)
					serverLogRepository = new ServerLogRepository(db);
				return serverLogRepository;
			}
		}

		public IRepository<License> Licenses
		{
			get
			{
				if (licenseRepository == null)
					licenseRepository = new LicenseRepository(db);
				return licenseRepository;
			}
		}


		public IRepository<LicenseUsersStatistic> LicenseStatistics
		{
			get
			{
				if (licenseStatisticRepository == null)
					licenseStatisticRepository = new LicenseStatisticRepository(db);
				return licenseStatisticRepository;
			}
		}



		public ISqlInjector SqlInjector
		{
			get
			{
				if (sqlInjector == null)
					sqlInjector = new SqlInjector(db);
				return sqlInjector;
			}
		}

		public void LazyLoadingEnabled(bool enabled)
		{
			db.Configuration.LazyLoadingEnabled = enabled;
		}

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					db.Dispose();
				}
				this.disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

}
