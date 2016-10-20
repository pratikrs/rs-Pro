using MyDBFDemo.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBFDemo.DataAccess.GenericRepository
{
    public class UnitOfWork : IDisposable
    {
        #region Declaration
        private DBFDemoEntities entities = null;
        private DbContextTransaction _transaction;
        #endregion

        #region Ctor
        public UnitOfWork()
        {
            //entities = new DBContext(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            entities = new DBFDemoEntities();

        }
        #endregion

        #region Create Repository Instance
        public T Repository<T>() where T : class
        {
            Object[] args = { entities, this };

            var repository = Activator.CreateInstance(typeof(T), args);
            return (T)repository;
        }

        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public IRepository<T> RepositoryInstance<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IRepository<T>;
            }
            IRepository<T> repo = new Repository<T>(entities, this);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        #endregion

        #region DB Transactions
        public void BeginTransaction()
        {
            _transaction = entities.Database.BeginTransaction();
        }
        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }
        public void Rollback()
        {
            _transaction.Rollback();
        }
        public void SaveChanges()
        {
            entities.SaveChanges();
        }
        #endregion

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
