using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Repositories;
using BugTracker.Repositories.Models;

namespace BugTracker.Services
{
    public class BugService : IBugService
    {
        private readonly IDatabase<Bug> _repository;

        public BugService(IDatabase<Bug> repository)
        {
            this._repository = repository;
        }

        public async Task<bool> OpenBug(Bug newBug)
        {
            if (string.IsNullOrEmpty(newBug.Title) || string.IsNullOrEmpty(newBug.Description))
                throw new ApplicationException("Missing mandatory bug data");

            newBug.Id = null;
            newBug.Status = Status.Opened;
            newBug.ReportedOn = DateTime.Now;

            return await _repository.Create(newBug);
        }

        public async Task<bool> CloseBug(string id)
        {
            ValidateBugId(id);

            var bug = await _repository.Find(id);
            ValidateBug(bug);
            bug.Status = Status.Closed;
            bug.ClosedOn = DateTime.Now;

            return await _repository.Update(bug);
        }

        public async Task<bool> UpdateBug(Bug bug)
        {
            ValidateBugId(bug.Id);

            var updatedBug = await _repository.Find(bug.Id);
            ValidateBug(updatedBug);

            updatedBug.ClosedOn = bug.ClosedOn;
            updatedBug.ReportedBy = bug.ReportedBy;
            updatedBug.ReportedOn = bug.ReportedOn;
            updatedBug.Severity = bug.Severity;
            updatedBug.Title = bug.Title;
            updatedBug.Description = bug.Description;
            updatedBug.Status = updatedBug.AssignedTo == bug.AssignedTo ? bug.Status : Status.Assigned;
            updatedBug.AssignedTo = bug.AssignedTo;

            return await _repository.Update(updatedBug);
        }

        public async Task<Bug> FindBug(string id)
        {
            ValidateBugId(id);

            var bug = await _repository.Find(id);

            return bug;
        }

        public async Task<List<Bug>> GetOpenBugs()
        {
            var bugs = await _repository.FindAll(_ => _.Status != Status.Closed);

            return bugs;
        }

        private void ValidateBugId(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                throw new ApplicationException("Invalid bug id");
        }

        private void ValidateBug(Bug bug)
        {
            if (bug == null)
                throw new ApplicationException("Invalid bug id");
        }
    }
}
