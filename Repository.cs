using MyDBFDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace MyDBFDemo.DataAccess.GenericRepository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DBFDemoEntities _context = null;
        private readonly DbSet<TEntity> _dbSet;
        UnitOfWork uom = null;
        public Repository(DBFDemoEntities context, UnitOfWork _uom)
        {
            _context = context;
            uom = _uom;
            var dbContext = context as DbContext;

            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
        }
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool> predicate = null)
        {
            if (predicate != null)
            {
                return _dbSet.Where(predicate);
            }

            return _dbSet.AsEnumerable();
        }

        public TEntity Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            uom.SaveChanges();
        }

        public void AddRange(List<TEntity> entity)
        {
            _dbSet.AddRange(entity);
            uom.SaveChanges();
        }

        public void Update(TEntity entityToUpdate)
        {
            var entry = _context.Entry(entityToUpdate);
            var key = this.GetPrimaryKey(entry);

            if (entry.State == EntityState.Detached)
            {
                var currentEntry = _dbSet.Find(key);
                if (currentEntry != null)
                {
                    var attachedEntry = _context.Entry(currentEntry);
                    attachedEntry.CurrentValues.SetValues(entityToUpdate);
                }
                else
                {
                    _dbSet.Attach(entityToUpdate);
                    entry.State = EntityState.Modified;
                }
            }
        }

        private int GetPrimaryKey(DbEntityEntry entry)
        {
            var myObject = entry.Entity;
            var property =
                myObject.GetType()
                    .GetProperties()
                    .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
            return (int)property.GetValue(myObject, null);
        }

        public void Attach(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            uom.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _dbSet.Remove(entity);
            uom.SaveChanges();
        }
        public void DeleteRange(List<TEntity> entity)
        {
            _dbSet.RemoveRange(entity);
            uom.SaveChanges();
        }
        public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return _context.Database.SqlQuery<TEntity>(query, parameters);
        }
    }
}
