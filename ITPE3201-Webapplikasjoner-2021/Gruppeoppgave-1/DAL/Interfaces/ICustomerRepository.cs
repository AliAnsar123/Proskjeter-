using System.Collections.Generic;
using System.Threading.Tasks;
using Gruppeoppgave_1.Models;

namespace Gruppeoppgave_1.DAL
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAll();
        Task<Customer> GetById(int id);
        Task<string> Create(Customer customer);
        Task<string> Update(int id, Customer customer);
        Task<string> Delete(int id);
    }
}