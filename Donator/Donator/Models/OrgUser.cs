using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Models
{
    public class OrgUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdminOfOrg { get; set; }

        public int NonProfitOrgId { get; set; }
        public virtual NPO NonProfitOrg { get; set; }
    }
}
