using Donator.Helpers;
using Donator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public interface INPORepo
    {
        Task<NPO> GetNPOById(int id);
        Task<PagedList<NPO>> GetNPOs(PagingParams paging);
        Task<NPO> CreateNewNPO(NPO npo);
        Task<bool> UpdateNPO(NPO npo);
        Task<bool> DeleteNPO(int id);
        Task<bool> SaveAll();
    }
}
