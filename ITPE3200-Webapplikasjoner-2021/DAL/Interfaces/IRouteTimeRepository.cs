using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gruppeoppgave_1.DAL
{
    public interface IRouteTimeRepository
    {
        Task<List<RouteTime>> GetAll();
        Task<List<RouteTime>> GetById(int id);
        Task<List<RouteTime>> GetByRouteId(int routeId);
        Task<string> Create(RouteTime routeTime);
        Task<string> Update(int id, RouteTime routeTime);
        Task<string> Delete(int id);
    }
}
