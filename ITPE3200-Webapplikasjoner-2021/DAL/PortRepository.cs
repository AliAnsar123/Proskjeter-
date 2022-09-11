using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using Gruppeoppgave_1.DAL;

namespace Gruppeoppgave_1.Controllers
{
    public class PortRepository : IPortRepository
    {
        private readonly DatabaseContext _db;

        public PortRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<Port>> GetAll()
        {
            return await _db.Ports.ToListAsync();
        }

        public async Task<Port> GetById(int id)
        {
            Port port = await _db.Ports.FindAsync(id);

            if (port == null)
            {
                throw new KeyNotFoundException("Port with id: " + id + " not found");
            }

            return port;
        }

        public async Task<string> Create(Port inputPort)
        {
            Port port = await _db.Ports.FirstOrDefaultAsync(p => p.Name == inputPort.Name);

            if (port != null)
            {
                throw new ArgumentException("Port with name: '" + port.Name + "' already exists");
            }
            else
            {
                port = new Port
                {
                    Name = inputPort.Name
                };

                await _db.Ports.AddAsync(port);
                await _db.SaveChangesAsync();

                return "Port with name: '" + port.Name + "' created";
            }
        }

        public async Task<string> Update(int id, Port inputPort)
        {
            Port port = await _db.Ports.FindAsync(id);

            if (port == null)
            {
                throw new ArgumentException("Port with id: '" + id + "' not found");
            }
            else
            {
                port.Name = inputPort.Name;

                await _db.SaveChangesAsync();

                return "Port with id: '" + id + "' updated";
            }
        }

        public async Task<string> Delete(int id)
        {
            Port port = await _db.Ports.FindAsync(id);

            if (port == null)
            {
                throw new KeyNotFoundException("Port with id: '" + id + "' not found");
            }
            else
            {
                _db.Ports.Remove(port);
                await _db.SaveChangesAsync();

                return "Port with id: '" + id + "' deleted";
            }
        }
    }
}
