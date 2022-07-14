using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace Subfloor.Data
{
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        private readonly DbContext _dbContext;
        public EFRepository(DbContext dbContext)
        {
            _dbContext = dbContext;            
        }

        public virtual T GetById(Guid id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T GetSingleBySpec(ISpecification<T> spec)    
        {
            return List(spec).FirstOrDefault();
        }

        public IEnumerable<T> List(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult
                            .Where(spec.Criteria)
                            .AsEnumerable();
        }

        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public void Insert(T entity)
        {
            try
            {
                _dbContext.Set<T>().Add(entity);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }

        public void Update(T entity)
        {
            try
            {
                //_dbContext.Set<T>().Attach(entity);

                //ejs 6/15/2018: I implemented the below, to replace the above, because i simply could not address the issue of EF only tracking 1 entity with key...
                //we may need a more robust IRepository at some point
                var existing = _dbContext.Set<T>().Find(entity.Id);                
                _dbContext.Entry(existing).CurrentValues.SetValues(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public void Delete(T entity)
        {
            //_dbContext.Set<T>().Remove(entity);
            //ejs 6/15/2018: Same issue as explained above in Update method
            var existing = _dbContext.Set<T>().Find(entity.Id);           
            _dbContext.Set<T>().Remove(existing);
            _dbContext.SaveChanges();
        }

        public void BulkInsert(List<T> entities)
        {
            _dbContext.BulkInsert(entities);
        }

        public void BulkDelete(List<T> entities)
        {
            _dbContext.BulkDelete(entities);
        }

        public void BulkUpdate(List<T> entities)
        {
            _dbContext.BulkUpdate(entities);
        }
    }
}
