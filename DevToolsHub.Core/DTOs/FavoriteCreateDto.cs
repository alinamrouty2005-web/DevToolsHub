using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class FavoriteCreateDto
    {
        [Required]
        public int ToolId { get; set; }

    }
}
