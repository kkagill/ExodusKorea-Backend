using ExodusKorea.Data.Interfaces;
using ExodusKorea.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExodusKorea.Data.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
            where T : class, new()
    {
        private readonly ExodusKoreaContext _context;
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;
        public static SqlConnection conn;

        public EntityBaseRepository(ExodusKoreaContext context,
                                    IConfiguration config,
                                    IOptions<AppSettings> appSettings)
        {
            _context = context;
            _config = config;
            _appSettings = appSettings.Value;
        }       

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public virtual async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsEnumerable();
        }

        public virtual T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }
        
        public virtual T GetSingle(Expression<Func<T, bool>> predicate, 
                                   params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefault();
        }
        
        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate,
                                             params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).AsEnumerable();
        }

        public virtual async Task AddAsync(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach(var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public virtual async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public virtual SqlConnection GetCorrectConnection()
        {
            string connectionString = null;

            if (_appSettings.Environment.Equals("Development"))
                connectionString = _config.GetConnectionString("ExodusKoreaAzure");
            else
                connectionString = _config.GetConnectionString("ExodusKoreaAzure");

            return new SqlConnection(connectionString);
        }
    }
}