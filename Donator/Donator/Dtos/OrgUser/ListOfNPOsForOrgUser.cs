using Donator.Dtos.NPO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Dtos.OrgUser
{
    public class ListOfNPOsForOrgUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int UserCount { get; set; }
    }
}
