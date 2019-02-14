using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Repositories;
using BugTracker.Repositories.Models;

namespace BugTracker.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabase<User> _repository;

        public UserService(IDatabase<User> repository)
        {
            this._repository = repository;
        }

        public async Task<bool> AddUser(User newUser)
        {
            if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Username))
                throw new ApplicationException("Missing mandatory user data");

            return await _repository.Create(newUser);
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _repository.FindAll();

            return users;
        }
    }
}
