using Donator.Dtos.OrgUser;
using Donator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public interface IOrgUserRepo
    {
        Task<List<OrgUser>> GetUsersForOrgByOrgId(int orgId);
        Task<OrgUser> GetUserInOrgByOrgIdAndUserId(int orgId, string userId);
        Task<List<ListOfNPOsForOrgUser>> GetListOfActiveNPOsUserIsAffiliatedWith(string userId);
        Task<bool> AddUserToOrgUserList(int orgId, string userId, bool isAdmin);
        Task<bool> RemoveUserFromOrgUserListAsync(int orgId, string userId);
        Task<bool> DeactivateUserFromOrgUserListAsync(int orgId, string userId);
        Task<bool> ActivateUserFromOrgUserListAsync(int orgId, string userId);
        Task<bool> MakeUserAdminOfOrgAsync(int orgId, string userId);
        Task<bool> RescindAdminPrivledgesOfUserAsync(int orgId, string userId);
        Task<int> GetCountOfAdminsForOrg(int orgId);
    }
}
