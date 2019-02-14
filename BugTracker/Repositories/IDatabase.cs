using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Repositories.Models;

namespace BugTracker.Repositories
{
    public interface IDatabase<T> where T : IModel
    {
        Task<bool> Create(T document);
        Task<bool> Update(T document);
        Task<T> Find(string id);
        Task<List<T>> FindAll();
        Task<List<T>> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> filter);
        Task<bool> Delete(string id);
    }
}