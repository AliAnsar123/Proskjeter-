using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Gruppeoppgave_1.Controllers;
using Gruppeoppgave_1.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gruppeoppgave_1.DAL
{
    public class UserRepository: IUserRepository
    {
        private DatabaseContext _db;
        private ILogger<UserController> _log;

        public UserRepository(DatabaseContext db, ILogger<UserController> log)
        {
            _db = db;
            _log = log;
        }

        public static byte[] GenerateSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);

            return salt;
        }

        public static byte[] GenerateHash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 1000,
                numBytesRequested: 32);
        }

        public async Task<bool> Login(InputUser inputUser)
        {
            try
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Username == inputUser.Username);

                byte[] hash = GenerateHash(inputUser.Password, user.Salt);

                if (hash.SequenceEqual(user.Password))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }
    }
}
