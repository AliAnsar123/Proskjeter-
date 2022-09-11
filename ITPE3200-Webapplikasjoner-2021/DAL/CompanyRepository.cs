using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Gruppeoppgave_1.Controllers;
using System;

namespace Gruppeoppgave_1.DAL
{
    public class CompanyRepository : ICompanyRepository
    {

        private readonly DatabaseContext _db;
        private ILogger<CompanyController> _log;

        public CompanyRepository(DatabaseContext db, ILogger<CompanyController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<List<Company>> GetAll()
        {
                return await _db.Companies.ToListAsync();
        }

        public async Task<Company> GetById(int id)
        {
            Company company = await _db.Companies.FindAsync(id);

            if (company == null)
            {
                throw new KeyNotFoundException("Company with id: " + id + " not found");
            }

            return company;
         }
       
        public async Task<string> Create(Company inputCompany)
        {
            Company company = await _db.Companies.FirstOrDefaultAsync(c => c.Name == inputCompany.Name);

            if (company != null)
            {
                throw new ArgumentException("Company with name: '" + company.Name + "' already exists");
            }
            else
            {
                company = new Company
                {
                    Name = inputCompany.Name
                };

                await _db.Companies.AddAsync(company);
                await _db.SaveChangesAsync();

                return "Company with name: '" + company.Name + "' created";
            }
        }

        public async Task<string> Update(int id, Company inputCompany)
        {
            Company company = await _db.Companies.FindAsync(id);

            if (company == null)
            {
                throw new KeyNotFoundException("Company with id: '" + id + "' not found");
            }
            else
            {
                company.Name = inputCompany.Name;

                await _db.SaveChangesAsync();

                return "Company with id: '" + id + "' updated";
            }
        }

        public async Task<string> Delete(int id)
        {
            Company company = await _db.Companies.FindAsync(id);

            if (company == null)
            {
                throw new KeyNotFoundException("Company with id: '" + id + "' not found");
            }
            else
            {
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();

                return "Company with id: '" + id + "' deleted";
            }
        }
    }
}
