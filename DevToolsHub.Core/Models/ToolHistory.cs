using System;
using System.Collections.Generic;
using System.Text;

namespace DevToolsHub.Core.Models
{
    public class ToolHistory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ToolId { get; set; }

        public int? ProjectId { get; set; }

        public string? InputData { get; set; }

        public string? OutputData { get; set; }

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;


        public User User { get; set; } = null!;

        public Tool Tool { get; set; } = null!;

        public Project? Project { get; set; }
    }
}
