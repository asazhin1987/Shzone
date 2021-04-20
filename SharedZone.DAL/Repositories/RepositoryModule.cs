using SharedZone.DAL.EF;
using SharedZone.DAL.Entities;
using SharedZone.DAL.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SharedZone.DAL.Repositories
{
	public class ClientRepository : Repository<Client>, IRepository<Client>
	{
		public ClientRepository(SharedZoneContext context) : base(context) { }
	}

	public class CollectionRepository : Repository<Collection>, IRepository<Collection>
	{
		public CollectionRepository(SharedZoneContext context) : base(context) { }
	}

	public class HourRepository : ReadOnlyRepository<Hour>, IReadOnlyRepository<Hour>
	{
		public HourRepository(SharedZoneContext context) : base(context) { }
	}

	public class IFCFileTypeRepository : ReadOnlyRepository<IFCFileType>, IReadOnlyRepository<IFCFileType>
	{
		public IFCFileTypeRepository(SharedZoneContext context) : base(context) { }
	}

	public class IFCIncludeBoundatyRepository : ReadOnlyRepository<IFCIncludeBoundaty>, IReadOnlyRepository<IFCIncludeBoundaty>
	{
		public IFCIncludeBoundatyRepository(SharedZoneContext context) : base(context) { }
	}

	public class IFCJobRepository : Repository<IFCJob>, IRepository<IFCJob>
	{
		public IFCJobRepository(SharedZoneContext context) : base(context) { }
	}

	public class JobLaunchRepository : Repository<JobLaunch>, IRepository<JobLaunch>
	{
		public JobLaunchRepository(SharedZoneContext context) : base(context) { }
	}

	public class MinuteRepository : ReadOnlyRepository<Minute>, IReadOnlyRepository<Minute>
	{
		public MinuteRepository(SharedZoneContext context) : base(context) { }
	}

	public class NavisConvertElementsPropertyRepository : ReadOnlyRepository<NavisConvertElementsProperty>, IReadOnlyRepository<NavisConvertElementsProperty>
	{
		public NavisConvertElementsPropertyRepository(SharedZoneContext context) : base(context) { }
	}

	public class NavisCoordinateRepository : ReadOnlyRepository<NavisCoordinate>, IReadOnlyRepository<NavisCoordinate>
	{
		public NavisCoordinateRepository(SharedZoneContext context) : base(context) { }
	}

	public class NavisJobRepository : Repository<NavisJob>, IRepository<NavisJob>
	{
		public NavisJobRepository(SharedZoneContext context) : base(context) { }
	}

	public class NavisViewRepository : ReadOnlyRepository<NavisView>, IReadOnlyRepository<NavisView>
	{
		public NavisViewRepository(SharedZoneContext context) : base(context) { }
	}

	public class RevitJobRepository : Repository<RevitJob>, IRepository<RevitJob>
	{
		public RevitJobRepository(SharedZoneContext context) : base(context) { }
	}

	public class RevitModelRepository : Repository<RevitModel>, IRepository<RevitModel>
	{
		public RevitModelRepository(SharedZoneContext context) : base(context) { }

		public override async Task DeleteAsync(int id)
		{
			var item = await GetAsync(id);
			if (item != null)
				await DeleteAsync(item);
		}

		public override async Task DeleteAsync(RevitModel item)
		{
			foreach (var child in item.RevitModels)
				await DeleteAsync(child);
			await base.DeleteAsync(item);
		}
	}

	public class RevitServerRepository : Repository<RevitServer>, IRepository<RevitServer>
	{
		public RevitServerRepository(SharedZoneContext context) : base(context) { }
	}

	public class RevitVersionRepository : Repository<RevitVersion>, IRepository<RevitVersion>
	{
		public RevitVersionRepository(SharedZoneContext context) : base(context) { }
	}

	public class WeekDayRepository : ReadOnlyRepository<WeekDay>, IReadOnlyRepository<WeekDay>
	{
		public WeekDayRepository(SharedZoneContext context) : base(context) { }
	}

	public class ServerLogRepository : Repository<ServerLog>, IRepository<ServerLog>
	{
		public ServerLogRepository(SharedZoneContext context) : base(context) { }
	}

	public class LicenseRepository : Repository<License>, IRepository<License>
	{
		public LicenseRepository(SharedZoneContext context) : base(context) { }

		public override async Task DeleteAsync(int id)
		{
			var lic = await GetAsync(id);
			if (lic != null)
				await base.DeleteAsync(lic);
		}
		public override License Get(int Id)
		{
			return base.GetAll().FirstOrDefault();
		}

		public override async Task<License> GetAsync(int id)
		{
			return await base.GetAll().FirstOrDefaultAsync();
		}

	}

	public class LicenseStatisticRepository : Repository<LicenseUsersStatistic>, IRepository<LicenseUsersStatistic>
	{
		public LicenseStatisticRepository(SharedZoneContext context) : base(context) { }
	}
}
