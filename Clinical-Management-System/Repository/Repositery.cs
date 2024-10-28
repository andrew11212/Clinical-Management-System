using Clinical_Management_System.Data;
using Clinical_Management_System.IRepositery;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Clinical_Management_System.Repository
{
	public class Repositery<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext context;

		internal DbSet<T> Set;
		public Repositery(ApplicationDbContext context)
		{
			this.context = context;
			this.Set = context.Set<T>();
		}

		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter =null, params string[] includes)
		{
			IQueryable<T> query = Set;
			if(Filter != null)
			{
			query=query.Where(Filter);
			}
			foreach (var include in includes) 
			{
				query = query.Include(include);
			}
			return query;
		}

		public T Get(Expression<Func<T, bool>> Filter, params string[] includes)
		{
			IQueryable<T> query = Set;

			query = query.Where(Filter);

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return query.FirstOrDefault();
		}

		public void Add(T entity)
		{
			Set.Add(entity);
		}

		public void Remove(T entity)
		{
			Set.Remove(entity);
		}

		public void RemoveRamge(IEnumerable<T> entity)
		{
			Set.RemoveRange(entity);
		}

		
	}
}
