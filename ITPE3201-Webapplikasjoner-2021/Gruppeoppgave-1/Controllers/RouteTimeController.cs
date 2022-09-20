using Microsoft.AspNetCore.Mvc;
using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Gruppeoppgave_1.DAL;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Gruppeoppgave_1.Controllers
{
    [Route("api/routetimes")]
    public class RouteTimeController : ControllerBase
    {
        private IRouteTimeRepository _routeTime_db;
        private ILogger<RouteTimeController> _log;

        public RouteTimeController(IRouteTimeRepository db, ILogger<RouteTimeController> log)
        {
            _routeTime_db = db;
            _log = log;
        }


        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _routeTime_db.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByRouteId(int routeId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                List<RouteTime> routeTimes = await _routeTime_db.GetByRouteId(routeId);

                return Ok(routeTimes);
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
        public async Task<ActionResult> Create([FromBody]RouteTime routeTime)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_loggedIn")))
            {
                return Unauthorized("Unauthorized");
            }

            try
            {
                // some overriding of the datamodel validation is necessary because of database/model relationships
                ModelState.Remove("Route.Company");
                ModelState.Remove("Route.Destination");
                ModelState.Remove("Route.Origin");

                if (!ModelState.IsValid)
                {
                    throw new ArgumentException(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                string response = await _routeTime_db.Create(routeTime);

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
        public async Task<ActionResult> Update(int id, [FromBody]RouteTime routeTime)
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

                string response = await _routeTime_db.Update(id, routeTime);

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

                string response = await _routeTime_db.Delete(id);

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
