using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class CollectionItemCreateDto
    {
        [Required]
        public int CollectionId { get; set; }

        [Required]
        public int ToolId { get; set; }
    }
}
