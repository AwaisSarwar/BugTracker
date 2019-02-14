using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Models;
using BugTracker.Repositories.Models;
using BugTracker.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BugTracker.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userService.GetUsers();

                return Ok(users);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                var status = await _userService.AddUser(user);

                return status ? new StatusCodeResult(201) : new StatusCodeResult(422);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
