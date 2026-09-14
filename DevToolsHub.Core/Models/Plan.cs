using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
    public class Plan
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int DurationInDays { get; set; }

        public bool IsActive { get; set; } = true;


        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
