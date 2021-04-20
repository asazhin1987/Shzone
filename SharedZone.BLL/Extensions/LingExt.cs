using SharedZone.DAL.Interfaces;
using SharedZone.DAL.Entities;
using SharedZone.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SharedZone.DAL.Abstract;
using System.Data.Entity;

namespace SharedZone.BLL.Extensions
{
	public static class LingExt
	{
		public static IQueryable<NamedUnitDTO> GetNamedUnitQuery(this IQueryable<NamedUnit> items) =>
			items.Select(x => new NamedUnitDTO() {Id = x.Id, Name = x.Name }).OrderBy(x => x.Name);

		public static IEnumerable<NamedUnitDTO> GetNamedUnit(this IQueryable<NamedUnit> items) =>
			items.GetNamedUnitQuery().ToList();

		public static async Task<IEnumerable<NamedUnitDTO>> GetNamedUnitAsync(this IQueryable<NamedUnit> items) =>
			await items.GetNamedUnitQuery().ToListAsync();
	}
}
