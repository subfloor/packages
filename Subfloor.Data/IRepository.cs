using System;
using System.Collections.Generic;

namespace Subfloor.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        T GetById(Guid id);
        T GetSingleBySpec(ISpecification<T> spec);
        IEnumerable<T> List();
        IEnumerable<T> List(ISpecification<T> spec);
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);

        //these use EFCore.BulkExtensions
        void BulkInsert(List<T> entities);
        void BulkDelete(List<T> entities);
        void BulkUpdate(List<T> entities);
    }
}
