using Microsoft.AspNetCore.Mvc;
using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gruppeoppgave_1.DAL;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Gruppeoppgave_1.Controllers
{
    [ApiController]
    [Route("api/ports")]
    public class PortController : ControllerBase
    {
        private IPortRepository _port_db;
        private ILogger<PortController> _log;

        public PortController(IPortRepository db, ILogger<PortController> log)
        {
            _port_db = db;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _port_db.GetAll());
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

                Port port = await _port_db.GetById(id);

                return Ok(port);
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
        public async Task<ActionResult> Create(Port port)
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

                string response = await _port_db.Create(port);

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
        public async Task<ActionResult> Update(int id, Port port)
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

                string response = await _port_db.Update(id, port);

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

                string response = await _port_db.Delete(id);

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
