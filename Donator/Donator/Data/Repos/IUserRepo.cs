using Donator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public interface IUserRepo
    {
        Task<User> GetUserById(string userId);
    }
}
