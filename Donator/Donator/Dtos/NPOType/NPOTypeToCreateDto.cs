using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Dtos.NPOType
{
    public class NPOTypeToCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string TaxCodeIdentifier { get; set; }
    }
}
