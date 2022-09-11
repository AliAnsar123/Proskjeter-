using System.Threading.Tasks;
using Gruppeoppgave_1.DAL;
using Gruppeoppgave_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gruppeoppgave_1.Controllers
{
    [Route("api/login")]
    public class UserController : ControllerBase
    {
        private IUserRepository _db;
        private ILogger<UserController> _log;

        public UserController(IUserRepository db, ILogger<UserController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody]InputUser inputUser)
        {
            if (ModelState.IsValid)
            {
                bool isUserValid = await _db.Login(inputUser);

                if (isUserValid)
                {
                    HttpContext.Session.SetString("_loggedIn", "true");
                    return Ok("Valid user credentials, logged in");
                }
            }

            _log.LogInformation("Login for user: " + inputUser.Username + " failed");
            HttpContext.Session.SetString("_loggedIn", "");
            return Unauthorized("Invalid user credentials");
        }

        [HttpDelete]
        public void Logout()
        {
            HttpContext.Session.SetString("_loggedIn", "");
        }
    }
}
