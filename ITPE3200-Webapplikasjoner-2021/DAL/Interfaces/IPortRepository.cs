using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gruppeoppgave_1.DAL
{
    public interface IPortRepository
    {
        Task<List<Port>> GetAll();
        Task<Port> GetById(int id);
        Task<string> Create(Port port);
        Task<string> Update(int id, Port port);
        Task<string> Delete(int id);
    }
}
