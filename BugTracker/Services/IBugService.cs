using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Repositories.Models;

namespace BugTracker.Services
{
    public interface IBugService
    {
        Task<bool> OpenBug(Bug newBug);
        Task<bool> CloseBug(string id);
        Task<Bug> FindBug(string id);
        Task<List<Bug>> GetOpenBugs();
        Task<bool> UpdateBug(Bug bug);
    }
}