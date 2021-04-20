using SharedZone.DAL.Abstract;
using SharedZone.DAL.EF;
using SharedZone.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SharedZone.DAL.Repositories
{
	public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : BimacadUnit
	{
		protected readonly SharedZoneContext db;
		protected DbSet<T> dbSet;

		public ReadOnlyRepository(SharedZoneContext context)
		{
			db = context;
			dbSet = context.Set<T>();
		}

		public virtual IEnumerable<T> Find(Func<T, bool> predicate)
		{
			return dbSet.AsNoTracking().Where(predicate).ToList();
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync()
		{
			return await dbSet.ToListAsync();
		}

		public virtual async Task<T> GetAsync(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public virtual T Get(int Id)
		{
			return dbSet.Find(Id);
		}

		public virtual async Task<IEnumerable<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
		{
			return await Include(includeProperties).ToListAsync();
		}

		public virtual IEnumerable<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = Include(includeProperties);
			return query.Where(predicate).ToList();
		}

		public virtual IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = dbSet.AsNoTracking();
			return includeProperties
				.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
		}

		public virtual T Get(string Key)
		{
			return dbSet.Find(Key);
		}



		public virtual IQueryable<T> GetAll()
		{
			return dbSet.AsQueryable();
		}
	}



	public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : BimacadUnit
	{

		public Repository(SharedZoneContext context) : base(context)
		{
		}

		public virtual async Task CreateAsync(T item)
		{
			dbSet.Add(item);
			await db.SaveChangesAsync();
		}

		public virtual async Task DeleteAsync(int id)
		{
			T item = await dbSet.FindAsync(id);
			if (item != null)
				dbSet.Remove(item);
			await db.SaveChangesAsync();
		}

		public virtual async Task DeleteAsync(T item)
		{
			dbSet.Remove(item);
			await db.SaveChangesAsync();
		}

		public virtual async Task UpdateAsync(T item)
		{
			db.Entry(item).State = EntityState.Modified;
			await db.SaveChangesAsync();
		}



	}
}
