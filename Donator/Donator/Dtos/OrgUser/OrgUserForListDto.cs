using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Dtos.OrgUser
{
    public class OrgUserForListDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdminOfOrg { get; set; }
    }
}
