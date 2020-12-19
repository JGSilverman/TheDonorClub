using Donator.Helpers;
using Donator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public class NPORepo : INPORepo
    {
        public readonly AppDbContext _dbContext;

        public NPORepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NPO> CreateNewNPO(NPO npo)
        {
            npo.CreatedOn = DateTime.Now;
            npo.LastUpdatedOn = DateTime.Now;
            _dbContext.Add(npo);
            await _dbContext.SaveChangesAsync();
            return npo;
        }

        public async Task<bool> DeleteNPO(int id)
        {
            var npo = await _dbContext.NonProfitOrgs.FirstOrDefaultAsync(c => c.Id == id);

            _dbContext.Remove(npo);

            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<NPO> GetNPOById(int id)
        {
            return await _dbContext.NonProfitOrgs
                                    .Include(x => x.Type)
                                    .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> SaveAll()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateNPO(NPO npo)
        {
            npo.LastUpdatedOn = DateTime.Now;
            _dbContext.Entry(npo).Property(x => x.Name).IsModified = true;
            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<PagedList<NPO>> GetNPOs(PagingParams paging)
        {
            var npos = _dbContext.NonProfitOrgs;
            return await PagedList<NPO>.CreateAsync(
                npos, paging.PageNumber, paging.PageSize
            );
        }
    }
}
