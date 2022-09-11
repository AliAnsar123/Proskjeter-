using Microsoft.AspNetCore.Mvc;
using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gruppeoppgave_1.DAL;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Gruppeoppgave_1.Controllers
{
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private IOrderRepository _order_db;
        private ILogger<OrderController> _log;

        public OrderController(IOrderRepository db, ILogger<OrderController> log)
        {
            _order_db = db;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_loggedIn")))
            {
                return Unauthorized("Unauthorized");
            }

            List<Order> orders = await _order_db.GetAll();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("_loggedIn")))
            {
                return Unauthorized("Unauthorized");
            }

            try
            {
                Order order = await _order_db.GetById(id);

                return Ok(order);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody]Order order)
        {
            // creating order does not require login because 'guest-users' should be able to send their orders

            try
            {
                // some overriding of the datamodel validation is necessary
                ModelState.Remove("ReturnRouteTime.Route");
                ModelState.Remove("DepartureRouteTime.Route");
                ModelState.Remove("MainCustomer.ZipCode.City");

                if (!ModelState.IsValid)
                {
                    throw new ArgumentException(string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                }

                string response = await _order_db.Create(order);

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
        public async Task<ActionResult> Update(int id, [FromBody]Order order)
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

                string response = await _order_db.Update(id, order);

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

                string response = await _order_db.Delete(id);

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
