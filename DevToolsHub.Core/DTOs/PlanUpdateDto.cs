using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class PlanUpdateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Range(1, 3650)]
        public int DurationInDays { get; set; }

        public bool IsActive { get; set; }
    }
}
