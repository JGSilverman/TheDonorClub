using Donator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Donator.Dtos.OrgUser;

namespace Donator.Data.Repos
{
    public class OrgUserRepo : IOrgUserRepo
    {
        public readonly AppDbContext _dbContext;

        public OrgUserRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrgUser> GetUserInOrgByOrgIdAndUserId(int orgId, string userId)
        {
            return await _dbContext.OrgUsers
                                    .Where(x => x.NonProfitOrgId == orgId && x.UserId == userId)
                                    .Include(x => x.User)
                                    .FirstOrDefaultAsync();
        }

        public async Task<List<OrgUser>> GetUsersForOrgByOrgId(int orgId)
        {
            return await _dbContext.OrgUsers
                                    .Where(x => x.NonProfitOrgId == orgId)
                                    .Include(x => x.User)
                                    .ToListAsync();
        }

        public async Task<bool> AddUserToOrgUserList(int orgId, string userId, bool isAdmin)
        {
            try
            {
                _dbContext.OrgUsers.Add(new OrgUser
                {
                    NonProfitOrgId = orgId,
                    UserId = userId,
                    IsActive = true,
                    IsAdminOfOrg = isAdmin
                });

                return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveUserFromOrgUserListAsync(int orgId, string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("User Id can't be null or empty");
            var user = await _dbContext.OrgUsers
                                        .Where(x => x.NonProfitOrgId == orgId)
                                        .FirstOrDefaultAsync(x => x.UserId == userId);

            _dbContext.Remove(user);
            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<bool> DeactivateUserFromOrgUserListAsync(int orgId, string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("User Id can't be null or empty");
            var user = await _dbContext.OrgUsers
                                        .Where(x => x.NonProfitOrgId == orgId)
                                        .FirstOrDefaultAsync(x => x.UserId == userId);

            user.IsActive = false;
            _dbContext.Entry(user).Property(x => x.IsActive).IsModified = true;

            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<bool> ActivateUserFromOrgUserListAsync(int orgId, string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("User Id can't be null or empty");
            var user = await _dbContext.OrgUsers
                                        .Where(x => x.NonProfitOrgId == orgId)
                                        .FirstOrDefaultAsync(x => x.UserId == userId);

            user.IsActive = true;
            _dbContext.Entry(user).Property(x => x.IsActive).IsModified = true;

            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<bool> MakeUserAdminOfOrgAsync(int orgId, string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("User Id can't be null or empty");
            var user = await _dbContext.OrgUsers
                                        .Where(x => x.NonProfitOrgId == orgId)
                                        .FirstOrDefaultAsync(x => x.UserId == userId);

            user.IsAdminOfOrg = true;
            _dbContext.Entry(user).Property(x => x.IsAdminOfOrg).IsModified = true;

            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<bool> RescindAdminPrivledgesOfUserAsync(int orgId, string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("User Id can't be null or empty");
            var user = await _dbContext.OrgUsers
                                        .Where(x => x.NonProfitOrgId == orgId)
                                        .FirstOrDefaultAsync(x => x.UserId == userId);

            user.IsAdminOfOrg = false;
            _dbContext.Entry(user).Property(x => x.IsAdminOfOrg).IsModified = true;

            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<int> GetCountOfAdminsForOrg(int orgId)
        {
            return await _dbContext.OrgUsers
                                    .Where(x => x.NonProfitOrgId == orgId && x.IsAdminOfOrg)
                                    .CountAsync();
        }

        public async Task<List<ListOfNPOsForOrgUser>> GetListOfActiveNPOsUserIsAffiliatedWith(string userId)
        {
            var npos = from u in _dbContext.OrgUsers
                       where u.UserId == userId && u.IsActive
                       join np in _dbContext.NonProfitOrgs on u.NonProfitOrgId equals np.Id
                       join t in _dbContext.NPOTypes on np.TypeId equals t.Id
                       select new ListOfNPOsForOrgUser
                       {
                           Id = np.Id,
                           Name = np.Name,
                           Type = t.Name,
                           UserCount = _dbContext.OrgUsers.Where(x => x.NonProfitOrgId == np.Id && x.IsActive).Count()
                       };

            return await npos.ToListAsync();
        }
    }
}
