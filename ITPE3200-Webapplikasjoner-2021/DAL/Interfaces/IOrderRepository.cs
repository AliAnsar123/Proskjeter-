using System.Collections.Generic;
using System.Threading.Tasks;
using Gruppeoppgave_1.Models;

namespace Gruppeoppgave_1.DAL
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAll();
        Task<Order> GetById(int id);
        Task<string> Create(Order order);
        Task<string> Update(int id, Order order);
        Task<string> Delete(int id);
    }
}
