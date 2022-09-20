using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Gruppeoppgave_1.DAL;
using System;

namespace Gruppeoppgave_1.Controllers
{
    public class CustomerRepository : ICustomerRepository

    {
        private readonly DatabaseContext _db;

        public CustomerRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<Customer>> GetAll()
        {
            return await _db.Customers.ToListAsync();
        }

        public async Task<Customer> GetById(int id)
        {
            Customer customer = await _db.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer with id: " + id + " not found");
            }

            return customer;
        }


        public async Task<string> Create(Customer inputCustomer)
        {
            Customer customer = await _db.Customers.FirstOrDefaultAsync(c => c.Email == inputCustomer.Email);

            if (customer != null)
            {
                throw new ArgumentException("Customer with email: '" + customer.Email + "' already exists");
            }
            else
            {
                ZipCode zipCode = await _db.ZipCodes.FindAsync(inputCustomer.ZipCode.Id);

                if (zipCode == null)
                {
                    throw new KeyNotFoundException("Zip code: " + inputCustomer.ZipCode.Id + " not found");
                }

                customer = new Customer
                {
                    FirstName = inputCustomer.FirstName,
                    LastName = inputCustomer.LastName,
                    Email = inputCustomer.Email,
                    Phone = inputCustomer.Phone,
                    Street = inputCustomer.Street,
                    ZipCode = zipCode
                };

                await _db.Customers.AddAsync(customer);
                await _db.SaveChangesAsync();

                return "Customer with email: '" + customer.Email + "' created";
            }
        }

        public async Task<string> Update(int id, Customer inputCustomer)
        {
            Customer customer = await _db.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer with id: '" + id + "' not found");
            }
            else
            {
                ZipCode zipCode = await _db.ZipCodes.FindAsync(inputCustomer.ZipCode.Id);

                if (zipCode == null)
                {
                    throw new KeyNotFoundException("Zip code: " + inputCustomer.ZipCode.Id + " not found");
                }

                customer.FirstName = inputCustomer.FirstName;
                customer.LastName = inputCustomer.LastName;
                customer.Email = inputCustomer.Email;
                customer.Phone = inputCustomer.Phone;
                customer.Street = inputCustomer.Street;
                customer.ZipCode = zipCode;

                await _db.SaveChangesAsync();

                return "Customer with id: '" + id + "' updated";
            }
        }

        public async Task<string> Delete(int id)
        {
            Customer customer = await _db.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer with id: '" + id + "' not found");
            }
            else
            {
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();

                return "Customer with id: '" + id + "' deleted";
            }
        }
    }
}