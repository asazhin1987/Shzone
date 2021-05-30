using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedZone.DAL.Interfaces
{
	public interface ISqlInjector
	{
		Task InsertModelsAsync(int collectionId, IEnumerable<int> models);

		Task ClearModelsAsync(int collectionId);
	}
}
