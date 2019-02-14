using System;
using BugTracker.Common;
using BugTracker.Repositories.Models;

namespace BugTracker.Repositories
{
    public class BugRepository : IBugRepository
    {
        private readonly IDatabase _db;

        public BugRepository(IDatabase db)
        {
            this._db = db;
        }

        public bool CreateBug(Bug bug)
        {
            bool isCreated = false;

            return isCreated;
        }
    }
}
