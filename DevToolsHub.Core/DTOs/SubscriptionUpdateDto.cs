using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class SubscriptionUpdateDto
    {
        [Required]
        public int PlanId { get; set; }

        public bool IsActive { get; set; }
    }
}
