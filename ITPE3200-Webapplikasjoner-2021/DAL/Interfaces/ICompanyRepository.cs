using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gruppeoppgave_1.DAL
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAll();
        Task<Company> GetById(int id);
        Task<string> Create(Company company);
        Task<string> Update(int id, Company company);
        Task<string> Delete(int id);
    }
}
