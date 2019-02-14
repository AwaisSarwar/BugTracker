using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Repositories.Models;

namespace BugTracker.Services
{
    public interface IUserService
    {
        Task<bool> AddUser(User newUser);
        Task<List<User>> GetUsers();
    }
}