using System.Threading.Tasks;
using Gruppeoppgave_1.Models;

namespace Gruppeoppgave_1.DAL
{
    public interface IUserRepository
    {
        Task<bool> Login(InputUser inputUser);
    }
}
