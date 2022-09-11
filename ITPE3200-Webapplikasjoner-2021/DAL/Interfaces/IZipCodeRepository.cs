using Gruppeoppgave_1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gruppeoppgave_1.DAL
{
    public interface IZipCodeRepository
    {
        Task<List<ZipCode>> GetAll();
        Task<ZipCode> GetById(string id);
        Task<string> Create(ZipCode zipCode);
        Task<string> Update(string id, ZipCode zipCode);
        Task<string> Delete(string id);
    }
}
