using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableauDataExtractor.Blob;

namespace TableauDataExtractor.DAL
{
    class GenericRepository<T> : IDisposable where T : class 
    {
        protected readonly TableauContext db = new TableauContext();

        public virtual void Create(T item)
        {
            db.Set<T>().Add(item);
            db.SaveChanges();
        }

        public virtual void Remove(T item)
        {
            db.Set<T>().Remove(item);
            db.SaveChanges();
        }

        public virtual void Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;

            db.SaveChanges();
        }

        public virtual T GetById(object id)
        {
            return db.Set<T>().Find(id);
        }

        public virtual int Count()
        {
            return db.Set<T>().Count();
        }

        public virtual IQueryable<T> All()
        {
            return db.Set<T>();
        }

        public virtual void AddOrUpdate(T item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;

                db.SaveChanges();

            }
            catch (DbUpdateConcurrencyException e)
            {
                db.Set<T>().Add(item);
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
