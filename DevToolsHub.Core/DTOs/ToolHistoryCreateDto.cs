using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class ToolHistoryCreateDto
    {
        [Required]
        public int ToolId { get; set; }

        public int? ProjectId { get; set; }

        [StringLength(10000)]
        public string? InputData { get; set; }

        [StringLength(10000)]
        public string? OutputData { get; set; }
    }
}
