using Donator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public class UserRepo : IUserRepo
    {
        public readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
