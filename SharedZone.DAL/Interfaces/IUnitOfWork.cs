using SharedZone.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedZone.DAL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<Collection> Collections { get; }
		IRepository<Client> Clients { get; }
		IReadOnlyRepository<Hour> Hours { get; }
		IReadOnlyRepository<IFCFileType> IFCFileTypes { get; }
		IReadOnlyRepository<IFCIncludeBoundaty> IFCIncludeBoundaties { get; }
		IRepository<IFCJob> IFCJobs { get; }
		IRepository<JobLaunch> JobLaunches { get; }
		IReadOnlyRepository<Minute> Minutes { get; }
		IReadOnlyRepository<NavisConvertElementsProperty> NavisConvertElementsProperties { get; }
		IReadOnlyRepository<NavisCoordinate> NavisCoordinates { get; }
		IRepository<NavisJob> NavisJobs { get; }
		IReadOnlyRepository<NavisView> NavisViews { get; }
		IRepository<RevitJob> RevitJobs { get; }
		IRepository<RevitModel> RevitModels { get; }
		IRepository<RevitServer> RevitServers { get; }
		IRepository<RevitVersion> RevitVersions { get; }
		IReadOnlyRepository<WeekDay> WeekDays { get; }
		IRepository<ServerLog> ServerLogs { get; }
		IRepository<License> Licenses { get; }
		IRepository<LicenseUsersStatistic> LicenseStatistics { get; }
	}
}
