using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Gruppeoppgave_1.DAL;

namespace Gruppeoppgave_1.Controllers
{
    public class RouteRepository : IRouteRepository
    {
        private readonly DatabaseContext _db;
        private ILogger<RouteController> _log;

        public RouteRepository(DatabaseContext db, ILogger<RouteController> log)
        {
            _db = db;
            _log = log;
        }


        public async Task<List<Route>> GetAll()
        {
            try
            {
                List<Route> allRoutes = await _db.Routes.ToListAsync();
                return allRoutes;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Route> GetById(int id)
        {
            Route route = await _db.Routes.FindAsync(id);

            if (route == null)
            {
                _log.LogInformation("Route with id: " + id + " not found");
                throw new KeyNotFoundException("Route with id: " + id + " not found");
            }
            return route;

        }

        public async Task<string> Create(Route inputRoute)
        {
            Route route = await _db.Routes.FirstOrDefaultAsync(r =>
                r.Origin.Id      == inputRoute.Origin.Id &&
                r.Destination.Id == inputRoute.Destination.Id &&
                r.Company.Id     == inputRoute.Company.Id
            );

            if (route != null)
            {
                throw new ArgumentException("Route with id: '" + route.Id + "' has the exact same properties");
            }
            else if (inputRoute.Origin.Id == inputRoute.Destination.Id)
            {
                throw new ArgumentException("Origin port and destination port cannot be the sam");
            }
            else
            {
                Company company  = await _db.Companies.FindAsync(inputRoute.Company.Id);
                Port destination = await _db.Ports.FindAsync(inputRoute.Destination.Id);
                Port origin      = await _db.Ports.FindAsync(inputRoute.Origin.Id);

                route = new Route() {
                    Company     = company     ?? throw new KeyNotFoundException("Company with id: '" + inputRoute.Company.Id + "' not found"),
                    Destination = destination ?? throw new KeyNotFoundException("Port with id: '" + inputRoute.Destination.Id + "' not found"),
                    Origin      = origin      ?? throw new KeyNotFoundException("Port with id: '" + inputRoute.Origin.Id + "' not found")
                };

                await _db.Routes.AddAsync(route);
                await _db.SaveChangesAsync();

                return "Route created";
            }
        }

        public async Task<string> Update(int id, Route inputRoute)
        {
            Route route = await _db.Routes.FindAsync(id);

            if (route == null)
            {
                throw new KeyNotFoundException("Route with id: '" + id + "' not found");
            }
            else
            {
                Company company = await _db.Companies.FindAsync(inputRoute.Company.Id);
                Port destination = await _db.Ports.FindAsync(inputRoute.Destination.Id);
                Port origin = await _db.Ports.FindAsync(inputRoute.Origin.Id);

                route.Company     = company     ?? throw new KeyNotFoundException("Company with id: '" + inputRoute.Company.Id + "' not found");
                route.Destination = destination ?? throw new KeyNotFoundException("Port with id: '" + inputRoute.Destination.Id + "' not found");
                route.Origin      = origin      ?? throw new KeyNotFoundException("Port with id: '" + inputRoute.Origin.Id + "' not found");

                await _db.SaveChangesAsync();

                return "Route with id: '" + id + "' updated";
            }
        }

        public async Task<string> Delete(int id)
        {
            Route route = await _db.Routes.FindAsync(id);

            if (route == null)
            {
                throw new KeyNotFoundException("Route with id: '" + id + "' not found");
            }
            else
            {
                _db.Routes.Remove(route);
                await _db.SaveChangesAsync();

                return "Route with id: '" + id + "' deleted";
            }
        }
    }
}
