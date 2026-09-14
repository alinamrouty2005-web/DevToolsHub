using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PlanId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;


        // One-to-One
        public User User { get; set; } = null!;

        // Many-to-One
        public Plan Plan { get; set; } = null!;
    }
}
