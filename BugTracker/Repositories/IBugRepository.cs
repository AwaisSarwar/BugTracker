using BugTracker.Repositories.Models;

namespace BugTracker.Repositories
{
    public interface IBugRepository
    {
        bool CreateBug(Bug bug);
    }
}