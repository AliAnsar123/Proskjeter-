using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gruppeoppgave_1.DAL
{
    public interface IRouteRepository
    {
        Task<List<Route>> GetAll();
        Task<Route> GetById(int id);
        Task<string> Create(Route route);
        Task<string> Update(int id, Route Route);
        Task<string> Delete(int id);
    }
}
