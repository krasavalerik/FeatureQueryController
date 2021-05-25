using System;
using WebApp.Common;

namespace WebApp.Repositorys
{
    public abstract class GenericRepository<T> where T : class
    {
		private readonly EfContext _context;

		public GenericRepository(EfContext context)
        {
			_context = context;
		}

		/// <summary>
		/// Поиск сущности по Id.
		/// </summary>
		/// <param name="id"> Id сущности указанного типа. </param>
		/// <returns> Сущность указанного типа. </returns>
		public virtual T Get(long id)
		{
			try
			{
				return _context.Set<T>().Find(id);
			}
			catch (Exception ex)
			{
				Logger.Error($"Не удалось получить объект типа {typeof(T)} с идентификатором = {id}.", ex);
				throw;
			}
		}
	}
}
