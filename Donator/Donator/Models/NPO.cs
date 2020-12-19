using System.Collections.Generic;

namespace Donator.Models
{
    public class NPO : Organization
    {
        public NPO()
        {
            Users = new List<OrgUser>();
        }
        public int TypeId { get; set; }
        public virtual NPOType Type { get; set; }
        public string TaxId { get; set; }
        public string CreatedByUserId { get; set; }
        public virtual User CreatedBy { get; set; }
        public ICollection<OrgUser> Users { get; set; }
    }
}
