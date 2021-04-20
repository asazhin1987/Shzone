using System;
using System.Collections.Generic;
using System.Linq;
using SharedZone.DAL.Abstract;
using System.Threading.Tasks;
using System.Linq.Expressions;


namespace SharedZone.DAL.Interfaces
{

	public interface IReadOnlyRepository<T> where T : BimacadUnit
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetAsync(int id);
		T Get(string Key);
		T Get(int Id);

		Task<IEnumerable<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties);
		IEnumerable<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
		IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties);
		IQueryable<T> GetAll();
	}

	public interface IRepository<T> : IReadOnlyRepository<T> where T : BimacadUnit
	{
		Task CreateAsync(T item);
		Task UpdateAsync(T item);
		Task DeleteAsync(int id);
		Task DeleteAsync(T item);
		
	}
}
