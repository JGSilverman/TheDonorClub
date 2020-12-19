using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Models
{
    public abstract class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string CoverImgUrl { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
