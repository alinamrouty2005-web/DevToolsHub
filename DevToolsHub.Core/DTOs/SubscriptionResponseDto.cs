using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.DTOs
{
    public class SubscriptionResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PlanId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
