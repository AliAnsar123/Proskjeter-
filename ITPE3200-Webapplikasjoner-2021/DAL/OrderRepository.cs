using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Gruppeoppgave_1.Controllers;
using System.Linq;

namespace Gruppeoppgave_1.DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseContext _db;
        private ILogger<OrderController> _log;

        public OrderRepository(DatabaseContext db, ILogger<OrderController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<List<Order>> GetAll()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            Order order = await _db.Orders.FindAsync(id);

            if (order == null)
            {
                _log.LogInformation("Order with id: " + id + " not found");
                throw new KeyNotFoundException("Order with id: " + id + " not found");
            }

            return order;
        }

        public async Task<string> Create(Order inputOrder)
        {
            RouteTime departureRouteTime = await _db.RouteTimes.FindAsync(inputOrder.DepartureRouteTime.Id);
            RouteTime returnRouteTime = await _db.RouteTimes.FindAsync(inputOrder?.ReturnRouteTime.Id);

            List<Customer> customers = inputOrder.Customers == null ? new List<Customer>() : inputOrder.Customers.ToList().Select(inputC =>
            {
                Customer customer = _db.Customers.FirstOrDefault(c => c.FirstName == inputC.FirstName && c.LastName == inputC.LastName);
                if (customer == null)
                {
                    customer = new Customer() { FirstName = inputC.FirstName, LastName = inputC.LastName };
                    _log.LogInformation("Customer(passenger) with name: " + inputC.FirstName + " " + inputC.LastName + " not found. Creating new customer");
                }
                return customer;
            }).ToList();

            Order order = new Order() {
                IsRoundTrip = inputOrder.IsRoundTrip,
                NumberOfVehicles = inputOrder.NumberOfVehicles,
                MainCustomer = await ValidateCustomer(inputOrder.MainCustomer),
                DepartureRouteTime = departureRouteTime ?? throw new KeyNotFoundException("Route time with id: " + inputOrder.DepartureRouteTime.Id + " not found"),
                Customers = customers
            };

            // -1 is used as a placeholder/getaround value in the frontend
            if (inputOrder.ReturnRouteTime.Id != -1)
            {
                order.ReturnRouteTime = returnRouteTime ?? throw new KeyNotFoundException("Route time with id: " + inputOrder.ReturnRouteTime.Id + " not found");
            }

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            return "Order created";
        }

        public async Task<string> Update(int id, Order inputOrder)
        {
            Order order = await _db.Orders.FindAsync(id);

            if (order != null)
            {
                throw new KeyNotFoundException("Order with id: '" + id + "' not found");
            }
            else
            {
                RouteTime departureRouteTime = await _db.RouteTimes.FindAsync(inputOrder.DepartureRouteTime.Id);
                RouteTime returnRouteTime = await _db.RouteTimes.FindAsync(inputOrder.ReturnRouteTime.Id);

                List<Customer> customers = inputOrder.Customers == null ? new List<Customer>() : inputOrder.Customers.ToList().Select(inputC =>
                {
                    Customer customer = _db.Customers.FirstOrDefault(c => c.FirstName == inputC.FirstName && c.LastName == inputC.LastName);
                    if (customer == null)
                    {
                        customer = new Customer() { FirstName = inputC.FirstName, LastName = inputC.LastName };
                        _log.LogInformation("Customer(passenger) with name: " + inputC.FirstName + " " + inputC.LastName + " not found. Creating new customer");
                    }
                    return customer;
                }).ToList();

                order.IsRoundTrip        = inputOrder.IsRoundTrip;
                order.NumberOfVehicles   = inputOrder.NumberOfVehicles;
                order.MainCustomer       = await ValidateCustomer(inputOrder.MainCustomer);
                order.DepartureRouteTime = departureRouteTime ?? throw new KeyNotFoundException("Route time with id: " + inputOrder.DepartureRouteTime.Id + " not found");
                order.ReturnRouteTime    = returnRouteTime ?? throw new KeyNotFoundException("Route time with id: " + inputOrder.ReturnRouteTime.Id + " not found");
                order.Customers          = customers;

                await _db.SaveChangesAsync();

                return "Order with id: '" + id + "' updated";
            }
        }

        public async Task<string> Delete(int id)
        {

            Order order = await _db.Orders.FindAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException("Order with id: '" + id + "' not found");
            }
            else
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();

                return "Order with id: '" + id + "' deleted";
            }
        }
        
        // helper
        private async Task<Customer> ValidateCustomer(Customer inputCustomer)
        {
            Customer customer = await _db.Customers.FirstOrDefaultAsync(c => c.Email == inputCustomer.Email);

            ZipCode zipCode = await _db.ZipCodes.FindAsync(inputCustomer.ZipCode.Id);

            if (zipCode == null)
            {
                throw new KeyNotFoundException("Zip code: " + zipCode.Id + " not found");
            }
            else if (customer == null)
            {
                customer = new Customer
                {
                    FirstName = inputCustomer.FirstName,
                    LastName = inputCustomer.LastName,
                    Email = inputCustomer.Email,
                    Phone = inputCustomer.Phone,
                    Street = inputCustomer.Street,
                    ZipCode = zipCode
                };

                _log.LogInformation("Customer with email: " + customer.Email + " not found. Creating new customer");
                _db.Customers.Add(customer);
            }
            else
            {
                customer.FirstName = inputCustomer.FirstName;
                customer.FirstName = inputCustomer.FirstName;
                customer.LastName = inputCustomer.LastName;
                customer.Email = inputCustomer.Email;
                customer.Phone = inputCustomer.Phone;
                customer.Street = inputCustomer.Street;
                customer.ZipCode = zipCode;

                _log.LogInformation("Customer with email: " + customer.Email + " already exists. Assigning order to existing customer and updated customer");
            }

            return customer;
        }
    }

}
