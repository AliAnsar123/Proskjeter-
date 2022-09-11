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
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _company_db;
        private ILogger<CompanyController> _log;

        public CompanyController(ICompanyRepository db, ILogger<CompanyController> log)
        {
            _company_db = db;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<Company> companies = await _company_db.GetAll();

            return Ok(companies);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // combines error messages to one string, e.g. Firstname is required; Phone is required.
                    throw new ArgumentException(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                Company company = await _company_db.GetById(id);

                return Ok(company);
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
        public async Task<ActionResult> Create([FromBody]Company company)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_loggedIn")))
            {
                return Unauthorized("Unauthorized");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                string response = await _company_db.Create(company);

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
        public async Task<ActionResult> Update(int id, [FromBody]Company company)
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

                string response = await _company_db.Update(id, company);

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
                string response = await _company_db.Delete(id);

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

