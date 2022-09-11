using Microsoft.AspNetCore.Mvc;
using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gruppeoppgave_1.DAL;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Gruppeoppgave_1.Controllers
{
    [Route("api/routes")]
    public class RouteController : ControllerBase
    {
        private IRouteRepository _route_db;
        private ILogger<RouteController> _log;

        public RouteController(IRouteRepository db, ILogger<RouteController> log)
        {
            _route_db = db;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _route_db.GetAll());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                Route route = await _route_db.GetById(id);

                return Ok(route);
            }
            catch (KeyNotFoundException e)
            {
                _log.LogInformation(e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]Route route)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_loggedIn")))
            {
                return Unauthorized("Unauthorized");
            }

            try
            {
                // some overriding of the datamodel validation is necessary
                ModelState.Remove("Company.Name");
                ModelState.Remove("Destination.Name");
                ModelState.Remove("Origin.Name");

                if (!ModelState.IsValid)
                {
                    throw new ArgumentException(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                string response = await _route_db.Create(route);

                _log.LogInformation(response);
                return Ok(response);
            }
            catch (KeyNotFoundException e)
            {
                _log.LogInformation(e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody]Route route)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_loggedIn")))
            {
                return Unauthorized("Unauthorized");
            }

            try
            {
                // some overriding of the datamodel validation is necessary
                ModelState.Remove("Company.Name");
                ModelState.Remove("Destination.Name");
                ModelState.Remove("Origin.Name");

                if (!ModelState.IsValid)
                {
                    throw new ArgumentException(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                string response = await _route_db.Update(id, route);
                return Ok(response);
            }
            catch (KeyNotFoundException e)
            {
                _log.LogInformation(e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_loggedIn")))
            {
                return Unauthorized("Unauthorized");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                string response = await _route_db.Delete(id);

                _log.LogInformation(response);
                return Ok(response);
            }
            catch (DbUpdateException e)
            {
                _log.LogInformation(e.Message);
                return BadRequest("The record could not be deleted due to it being referenced by other database record(s). Remove reference before deleting");
            }
            catch (KeyNotFoundException e)
            {
                _log.LogInformation(e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
