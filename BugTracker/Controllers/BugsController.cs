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
    [Route("bugs")]
    public class BugsController : Controller
    {
        private readonly IBugService _bugService;

        public BugsController(IBugService bugService)
        {
            this._bugService = bugService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var openBugs = await _bugService.GetOpenBugs();

                return Ok(openBugs);
            }
            catch(Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var bug = await _bugService.FindBug(id);

                return Ok(bug);
            }
            catch(Exception)
            {
                return new StatusCodeResult(500); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Bug bug)
        {
            try
            {
                var status = await _bugService.OpenBug(bug);

                return status ? new StatusCodeResult(201) : new StatusCodeResult(422);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPatch("{id}/close")]
        public async Task<IActionResult> CloseBug(string id)
        {
            try
            {
                var status = await _bugService.CloseBug(id);

                return status ? new StatusCodeResult(200) : new StatusCodeResult(501);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Bug bug)
        {
            try
            {
                var status = await _bugService.UpdateBug(bug);

                return status ? new StatusCodeResult(200) : new StatusCodeResult(501);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
