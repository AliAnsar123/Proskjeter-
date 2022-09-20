using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gruppeoppgave_1.DAL
{
    public class ZipCodeRepository : IZipCodeRepository
    {
        private readonly DatabaseContext _db;

        public ZipCodeRepository(DatabaseContext db)
        {
            _db = db;
        }


        public async Task<List<ZipCode>> GetAll()
        {
            return await _db.ZipCodes.ToListAsync();
        }

        public async Task<ZipCode> GetById(string id)
        {
            ZipCode zipCode = await _db.ZipCodes.FindAsync(id);

            if (zipCode == null)
            {
                throw new KeyNotFoundException("Zip code with id: " + id + " not found");
            }

            return zipCode;
        }

        public async Task<string> Create(ZipCode inputZipCode)
        {
            ZipCode zipCode = await _db.ZipCodes.FindAsync(inputZipCode.Id);

            if (zipCode != null)
            {
                throw new ArgumentException("Zip code with id: '" + zipCode.Id + "' already exists");
            }
            else
            {
                // need to include Id here because it is not auto incremet/generated automatically
                zipCode = new ZipCode
                {
                    Id   = inputZipCode.Id,
                    City = inputZipCode.City
                };

                await _db.ZipCodes.AddAsync(zipCode);
                await _db.SaveChangesAsync();

                return "Zip code with id: '" + zipCode.Id + "' created";
            }
        }

        public async Task<string> Update(string id, ZipCode inputZipCode)
        {
            ZipCode zipCode = await _db.ZipCodes.FindAsync(id);

            if (zipCode != null)
            {
                throw new ArgumentException("Zip code with id: '" + id + "' not found");
            }
            else
            {
                zipCode.City = zipCode.City;

                await _db.SaveChangesAsync();

                return "Zip code with id: '" + id + "' updated";
            }
        }

        public async Task<string> Delete(string id)
        {
            ZipCode zipCode = await _db.ZipCodes.FindAsync(id);

            if (zipCode == null)
            {
                throw new KeyNotFoundException("Zip code with id: '" + id + "' not found");
            }
            else
            {
                _db.ZipCodes.Remove(zipCode);
                await _db.SaveChangesAsync();

                return "Zip code with id: '" + id + "' deleted";
            }
        }
    }
}
