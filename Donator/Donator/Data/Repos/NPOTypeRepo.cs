using Donator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public class NPOTypeRepo : INPOTypeRepo
    {
        public readonly AppDbContext _dbContext;

        public NPOTypeRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<NPOType> CreateNPOType(NPOType type)
        {
            _dbContext.Add(type);
            await _dbContext.SaveChangesAsync();
            return type;
        }

        public async Task<bool> DeleteNPOType(int id)
        {
            var type = await _dbContext.NPOTypes.FirstOrDefaultAsync(c => c.Id == id);

            _dbContext.Remove(type);

            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<List<NPOType>> GetAllNPOTypes()
        {
            return await _dbContext.NPOTypes.ToListAsync();
        }

        public async Task<NPOType> GetNPOTypeById(int id)
        {
            return await _dbContext.NPOTypes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> SaveAll()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateNPOType(NPOType type)
        {
            _dbContext.Entry(type).Property(x => x.Name).IsModified = true;
            _dbContext.Entry(type).Property(x => x.TaxCodeIdentifier).IsModified = true;
            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }
    }
}
