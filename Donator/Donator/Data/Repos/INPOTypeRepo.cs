using Donator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public interface INPOTypeRepo
    {
        Task<NPOType> GetNPOTypeById(int id);
        Task<List<NPOType>> GetAllNPOTypes();
        Task<NPOType> CreateNPOType(NPOType type);
        Task<bool> UpdateNPOType(NPOType type);
        Task<bool> DeleteNPOType(int id);
        Task<bool> SaveAll();
    }
}
