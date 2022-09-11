using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Gruppeoppgave_1.DAL;
using System.Linq;

namespace Gruppeoppgave_1.Controllers
{
    public class RouteTimeRepository : IRouteTimeRepository
    {
        private readonly DatabaseContext _db;
        private ILogger<RouteTimeController> _log;
        public RouteTimeRepository(DatabaseContext db, ILogger<RouteTimeController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<List<RouteTime>> GetAll()
        {
            try
            {
                List<RouteTime> allRouteTimes = await _db.RouteTimes.ToListAsync();
                return allRouteTimes;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<RouteTime>> GetById(int id)
        {
            List<RouteTime> routeTimes = await _db.RouteTimes.Where(rt => rt.Route.Id == id).ToListAsync();

            if (routeTimes == null)
            {
                _log.LogInformation("Route times for route with id: " + id + " not found");
                throw new KeyNotFoundException("Route times for route with id: " + id + " not found");
            }

            return routeTimes;
        }

        public async Task<List<RouteTime>> GetByRouteId(int routeId)
        {
            List<RouteTime> routeTimes = await _db.RouteTimes.Where(rt => rt.Route.Id == routeId).ToListAsync();

            if (routeTimes == null)
            {
                _log.LogInformation("Route times for route with id: " + routeId + " not found");
                throw new KeyNotFoundException("Route times for route with id: " + routeId + " not found");
            }

            return routeTimes;
        }

        public async Task<string> Create(RouteTime inputRouteTime)
        {
            Route route = await _db.Routes.FindAsync(inputRouteTime.Route.Id);

            if (route == null)
            {
                throw new ArgumentException("Route with id: '" + inputRouteTime.Route.Id + "' not found");
            }

            RouteTime routeTime = new RouteTime
            {
                Date      = inputRouteTime.Date,
                Direction = inputRouteTime.Direction,
                Price     = inputRouteTime.Price,
                Route     = route
            };

            await _db.RouteTimes.AddAsync(routeTime);
            await _db.SaveChangesAsync();

            return "Route time created";
        }

        public async Task<string> Update(int id, RouteTime inputRouteTime)
        {
            RouteTime routeTime = await _db.RouteTimes.FindAsync(id);

            if (routeTime != null)
            {
                throw new KeyNotFoundException("Route time with id: '" + id + "' not found");
            }
            else
            {
                Route route = await _db.Routes.FindAsync(inputRouteTime.Route.Id);

                routeTime.Date      = inputRouteTime.Date;
                routeTime.Direction = inputRouteTime.Direction;
                routeTime.Price     = inputRouteTime.Price;
                routeTime.Route     = route ?? throw new KeyNotFoundException("Route with id: '" + inputRouteTime.Route.Id + "' not found");

                await _db.SaveChangesAsync();

                return "Route time with id: '" + id + "' updated";
            }
        }

        public async Task<string> Delete(int id)
        {
            RouteTime routeTime = await _db.RouteTimes.FindAsync(id);

            if (routeTime == null)
            {
                throw new KeyNotFoundException("Route time with id: '" + id + "' not found");
            }
            else
            {
                _db.RouteTimes.Remove(routeTime);
                await _db.SaveChangesAsync();

                return "Route time with id: '" + id + "' deleted";
            }
        }
    }
}
