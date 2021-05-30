using SharedZone.DAL.EF;
using SharedZone.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedZone.DAL.Repositories
{
	public class SqlInjector : ISqlInjector
	{
		protected readonly SharedZoneContext db;

		public SqlInjector(SharedZoneContext context)
		{
			db = context;
		}

		public async Task InsertModelsAsync(int collectionId, IEnumerable<int> models)
		{
			foreach (var model in models)
			{
				await db.Database.ExecuteSqlCommandAsync(System.Data.Entity.TransactionalBehavior.EnsureTransaction,
					$"INSERT INTO [RevitModelsCollections] SELECT '{model}', {collectionId}");
			}
			
		}

		public async Task ClearModelsAsync(int collectionId)
		{
			await db.Database.ExecuteSqlCommandAsync(System.Data.Entity.TransactionalBehavior.EnsureTransaction,
				$"DELETE FROM [RevitModelsCollections] WHERE [CollectionId] = '{collectionId}'");
		}

		//public async Task ClearStatisticAsync(int monthsCount)
		//{
		//	await db.Database.ExecuteSqlCommandAsync(System.Data.Entity.TransactionalBehavior.EnsureTransaction,
		//		$"  DELETE FROM [StatisticEventsImplements] WHERE [SDateDate] < DATEADD(month, {monthsCount}, GETDATE())");
		//}

		//public async Task ClearRouteLogsAsync(int monthsCount)
		//{
		//	await db.Database.ExecuteSqlCommandAsync(System.Data.Entity.TransactionalBehavior.EnsureTransaction,
		//		$"  DELETE FROM [RouteLogs] WHERE [DateDate] < DATEADD(month, {monthsCount}, GETDATE())");
		//}
	}
}
