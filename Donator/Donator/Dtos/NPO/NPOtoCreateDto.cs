using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Dtos.NPO
{
    public class NPOtoCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string WebsiteUrl { get; set; }
        [Required]
        public string TaxId { get; set; }

    }
}
